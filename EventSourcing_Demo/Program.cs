using EventSourcing_Demo;
using System.ComponentModel.DataAnnotations;

var warehouseProductRepository = new WarehouseProductRepository();
var projection = new Projection(new ProductDbContext());
//warehouseProductRepository.Published += projection.RecieveEvent;
var key = string.Empty;
while(key != "X")
{
    Console.WriteLine("R: Receive Inventory");
    Console.WriteLine("S: Ship Inventory");
    Console.WriteLine("A: Adjust Inventory");
    Console.WriteLine("Q: Quantity On Hand");
    Console.WriteLine("E: Events");
    Console.Write("> ");
    key = Console.ReadLine()!.ToUpperInvariant();
    Console.WriteLine();

    var sku = Console.ReadLine()!;
    var warehouseProduct = warehouseProductRepository.Get(sku);

    switch (key)
    {
        case "R":
            var receiveInput = Console.ReadLine()!;

            if(receiveInput.IsValid())
            {
                warehouseProduct.RecieveProduct(receiveInput.Quantity());
                Console.WriteLine($"{sku} Recevied: {receiveInput.Quantity()}");
            }
            break;

        case "S":
            var shippedInput = Console.ReadLine()!;

            if (shippedInput.IsValid())
            {
                warehouseProduct.ShipProduct(shippedInput.Quantity());
                Console.WriteLine($"{sku} Shipped: {shippedInput.Quantity()}");
            }
            break;

        case "A":
            var adjustmentInput = Console.ReadLine()!;

            if (adjustmentInput.IsValid())
            {
                var reason = Console.ReadLine()!;
                warehouseProduct.AdjustInventory(adjustmentInput.Quantity(), reason);
                Console.WriteLine($"{sku} Asjusted: {adjustmentInput.Quantity()}");
            }
            break;

        case "Q":
            var quantity = warehouseProduct.GetQuantityOnHand();
            Console.WriteLine($"{sku} Quantity On Hand: {quantity}");
            break;

        case "E":
            Console.WriteLine($"Events: {sku}");
            foreach(var evnt in warehouseProduct.GetEvents())
            {
                switch (evnt)
                {
                    case ProductShipped productShipped:
                        Console.WriteLine($"{productShipped.DateTime} {sku} Shipped: {productShipped.Quantity}");
                        break;

                    case ProductRecieved productRecieved:
                        Console.WriteLine($"{productRecieved.DateTime} {sku} Received: {productRecieved.Quantity}");
                        break;

                    case InventoryAdjusted inventoryAdjusted:
                        Console.WriteLine($"{inventoryAdjusted.DateTime} {sku} Adjusted: {inventoryAdjusted.Quantity}");
                        break;
                }
            }
            break;
    }
    warehouseProductRepository.save(warehouseProduct);
    Console.ReadLine();
    Console.WriteLine();

}
