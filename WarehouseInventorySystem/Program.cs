using System;
using System.Collections.Generic;
using System.Linq;

// Custom exception for when an item is not found
public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(string message) : base(message) { }
}

// Generic repository with basic CRUD
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
