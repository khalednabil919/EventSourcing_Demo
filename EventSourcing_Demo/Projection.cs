using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing_Demo
{
    public class Projection
    {
        private readonly ProductDbContext _dbContext;
        public Projection(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void RecieveEvent(Evnt evnt)
        {
            switch(evnt.Event)
            {
                case ProductShipped productShipped:
                    Apply(productShipped);
                    break;

                case ProductRecieved productRecieved:
                    Apply(productRecieved);
                    break;
            }
        }

        public Product GetProduct(string sku)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Sku == sku);
            if (product == null)
            {
                _dbContext.Add(new Product { Sku = sku });
            }
            return product;
        }

        private void Apply(ProductShipped productShipped)
        {
            var product = GetProduct(productShipped.sku);
            product.Shipped += productShipped.Quantity;
            _dbContext.SaveChanges();
        }
        private void Apply(ProductRecieved productRecieved)
        {
            var product = GetProduct(productRecieved.sku);
            product.Received += productRecieved.Quantity;
            _dbContext.SaveChanges();
        }
    }
}
