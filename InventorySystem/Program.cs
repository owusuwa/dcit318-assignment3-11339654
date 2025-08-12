using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// Marker Interface
public interface IInventoryEntity
{
    int Id { get; }
}

// Immutable Record
public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

// Generic Inventory Logger
public class InventoryLogger<T> where T : IInventoryEntity
{
    private List<T> _log = new List<T>();
    private string _filePath;

    public InventoryLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void Add(T item)
    {
        _log.Add(item);
    }

    public List<T> GetAll()
    {
        return _log;
    }

    public void SaveToFile()
    {
        try
        {
            string json = JsonSerializer.Serialize(_log, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
            Console.WriteLine("Data saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
        }
    }

    public void LoadFromFile()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _log = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
                Console.WriteLine("Data loaded successfully.");
            }
            else
            {
                Console.WriteLine("File not found. No data loaded.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading file: {ex.Message}");
        }
    }
}

// Inventory App
public class InventoryApp
{
    private InventoryLogger<InventoryItem> _logger;

    public InventoryApp(string filePath)
    {
        _logger = new InventoryLogger<InventoryItem>(filePath);
    }

    public void SeedSampleData()
    {
        _logger.Add(new InventoryItem(1, "Laptop", 20, DateTime.Now));
        _logger.Add(new InventoryItem(2, "Mouse", 15, DateTime.Now));
        _logger.Add(new InventoryItem(3, "Keyboard", 12, DateTime.Now));
        _logger.Add(new InventoryItem(4, "Monitor", 5, DateTime.Now));
    }

    public void SaveData()
    {
        _logger.SaveToFile();
    }

    public void LoadData()
    {
        _logger.LoadFromFile();
    }

    public void PrintAllItems()
    {
        var items = _logger.GetAll();
        foreach (var item in items)
        {
            Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}, Date Added: {item.DateAdded}");
        }
    }
}

// Main Program
class Program
{
    static void Main(string[] args)
    {
        string filePath = "inventory.json";

        InventoryApp app = new InventoryApp(filePath);

        // Seed and save
        app.SeedSampleData();
        app.SaveData();

        // Simulate new session
        Console.WriteLine("\n--- Clearing memory and loading from file ---\n");
        app = new InventoryApp(filePath);
        app.LoadData();
        app.PrintAllItems();
    }
}

