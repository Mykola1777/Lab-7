using System;
using System.Collections.Generic;

public class Repository<T>
{
    private List<T> items = new List<T>();

    public void Add(T item)
    {
        items.Add(item);
    }

    public List<T> Find(Predicate<T> criteria)
    {
        return items.FindAll(criteria);
    }
}

class Program
{
    static void Main()
    {
        Repository<string> repository = new Repository<string>();
        repository.Add("apple");
        repository.Add("banana");
        repository.Add("cherry");
        repository.Add("date");

        List<string> results = repository.Find(item => item.StartsWith("b"));
        foreach (var result in results)
        {
            Console.WriteLine(result);
        }
    }
}

