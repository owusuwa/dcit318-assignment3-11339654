using System;
using System.Collections.Generic;
using System.Linq;

// ======================
// Custom Exception
// ======================
public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(string message) : base(message) { }
}

// ======================
// Generic Repository
// ======================
public class Repository<T>
{
    private List<T> items = new List<T>();

    public void Add(T item)
    {
        items.Add(item);
    }

    public List<T> GetAll()
    {
        return items;
    }

    public T Get(Func<T, bool> predicate)
    {
        var item = items.FirstOrDefault(predicate);
        if (item == null)
            throw new ItemNotFoundException("Item not found in repository.");
        return item;
    }

    public bool Remove(Func<T, bool> predicate)
    {
        var item = items.FirstOrDefault(predicate);
        if (item != null)
        {
            items.Remove(item);
            return true;
        }
        throw new ItemNotFoundException("Cannot remove: Item not found in repository.");
    }
}

// ======================
// Product and Item Types
// ======================
public abstract class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Product(int id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    public abstract void DisplayInfo();
}

public class ElectronicItem : Product
{
    public int WarrantyMonths { get; set; }

    public ElectronicItem(int id, string name, decimal price, int warrantyMonths)
        : base(id, name, price)
    {
        WarrantyMonths = warrantyMonths;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"[Electronic] ID: {Id}, Name: {Name}, Price: GHS {Price}, Warranty: {WarrantyMonths} months");
    }
}

public class GroceryItem : Product
{
    public DateTime ExpiryDate { get; set; }

    public GroceryItem(int id, string name, decimal price, DateTime expiryDate)
        : base(id, name, price)
    {
        ExpiryDate = expiryDate;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"[Grocery] ID: {Id}, Name: {Name}, Price: GHS {Price}, Expiry: {ExpiryDate.ToShortDateString()}");
    }
}

// ======================
// Program Entry Point
// ======================
class Program
{
    static void Main(string[] args)
    {
        var inventory = new Repository<Product>();

        try
        {
            // Add sample electronic items
            inventory.Add(new ElectronicItem(1, "Laptop", 4500.00m, 24));
            inventory.Add(new ElectronicItem(2, "PS5", 8500.00m, 12));

            // Add sample grocery items
            inventory.Add(new GroceryItem(3, "Virgin Oil", 65.79m, DateTime.Now.AddDays(7)));
            inventory.Add(new GroceryItem(4, "Rice", 200.00m, DateTime.Now.AddMonths(6)));

            Console.WriteLine("All Products in Inventory:");
            foreach (var item in inventory.GetAll())
            {
                item.DisplayInfo();
            }

            Console.WriteLine("\nFetching product with ID 1:");
            var product = inventory.Get(p => p.Id == 1);
            product.DisplayInfo();

            Console.WriteLine("\nAttempting to remove product with ID 99 (should throw exception):");
            inventory.Remove(p => p.Id == 99);
        }
        catch (ItemNotFoundException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected Error: {ex.Message}");
        }
    }
}
