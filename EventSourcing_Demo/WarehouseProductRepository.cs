using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing_Demo
{
    public class Evnt:EventArgs
    {
        public IEvent Event { get; set; } = null!;
    }
    public class WarehouseProductRepository
    {
        // 1- define a delagate
        // 2- 
        //public delegate void Publisher(Evnt args);
        //public event Publisher Published;

        private readonly Dictionary<string, IList<IEvent>> _inMemoryStreams = new();
        public WarehouseProduct Get(string sku)
        {
            var warehouseProduct = new WarehouseProduct(sku);
            if(_inMemoryStreams.ContainsKey(sku))
            {
                foreach(var evnt in _inMemoryStreams[sku])
                {
                    warehouseProduct.AddEvent(evnt);
                }
            }
            return warehouseProduct;
        }
        public void save(WarehouseProduct warehouseProduct)
        {
            _inMemoryStreams[warehouseProduct.Sku] = warehouseProduct.GetEvents();
        }
        //protected virtual void onPublished(Evnt evnt)
        //{
        //    if(Published != null)
        //        Published(evnt);
        //}
    }
}
