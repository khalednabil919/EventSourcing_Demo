using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing_Demo
{
    public interface IEvent { }

    public record ProductShipped(string sku, int Quantity, DateTime DateTime):IEvent;
    public record ProductRecieved(string sku, int Quantity, DateTime DateTime):IEvent;
    public record InventoryAdjusted(string sku, int Quantity,string Reason , DateTime DateTime):IEvent;

}
