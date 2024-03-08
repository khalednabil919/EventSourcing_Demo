using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing_Demo
{
    public class CurrentState { public int QuantityOnHand { get; set; } }
    public class WarehouseProduct
    {
        public string Sku { get; }
        private readonly IList<IEvent> _events = new List<IEvent>();
        private readonly CurrentState _currentState = new ();
        public WarehouseProduct(string sku)
        {
            Sku = sku;
        }
        
        public void ShipProduct(int quantity)
        {
            if (quantity > _currentState.QuantityOnHand)
                throw new InvalidDomainException("We don't have This Quantity");

            AddEvent(new ProductShipped(Sku, quantity, DateTime.UtcNow));
        }
        public void RecieveProduct(int quantity)
        {
            AddEvent(new ProductRecieved(Sku, quantity, DateTime.UtcNow));
        }
        public void AdjustInventory(int quantity, string reason)
        {
            if (_currentState.QuantityOnHand + quantity < 0)
                throw new InvalidDomainException("Cannot Adjust By Minus");

            AddEvent(new InventoryAdjusted(Sku, quantity, reason, DateTime.UtcNow));
        }
        private void Apply(ProductShipped productShipped)
        {
            _currentState.QuantityOnHand -= productShipped.Quantity;
        }
        private void Apply(ProductRecieved productRecieved)
        {
            _currentState.QuantityOnHand += productRecieved.Quantity;

        }
        private void Apply(InventoryAdjusted inventoryAdjusted)
        {
            _currentState.QuantityOnHand += inventoryAdjusted.Quantity;
        }
        public IList<IEvent> GetEvents()
        {
            return _events;
        }

        public void AddEvent(IEvent evnt)
        {
            switch(evnt)
            {
                case ProductShipped shipProduct:
                    Apply(shipProduct);
                    break;
                
                case ProductRecieved productRecieved:
                    Apply(productRecieved);
                    break;

                case InventoryAdjusted inventoryAdjusted:
                    Apply(inventoryAdjusted);
                    break;

                default:
                    throw new InvalidOperationException("UnSupportedType");
            }
            _events.Add(evnt);


        }
        public int GetQuantityOnHand() { return  _currentState.QuantityOnHand; }

        public class InvalidDomainException:Exception
        {
            public InvalidDomainException(string message):base(message)
            {
                
            }
        }
    }
}
