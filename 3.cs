using System;
using System.Collections.Generic;
using System.Threading;

public class FunctionCache<TKey, TResult>
{
    private readonly Dictionary<TKey, TResult> cache = new Dictionary<TKey, TResult>();
    private readonly TimeSpan cacheDuration;

    public FunctionCache(TimeSpan cacheDuration)
    {
        this.cacheDuration = cacheDuration;
    }

    public TResult GetOrAdd(TKey key, Func<TKey, TResult> function)
    {
        lock (cache)
        {
            if (cache.TryGetValue(key, out var cachedResult))
            {
                return cachedResult;
            }

            var result = function(key);
            cache[key] = result;

            // Очищення кешу після закінчення терміну дії
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Thread.Sleep(cacheDuration);
                lock (cache)
                {
                    cache.Remove(key);
                }
            });

            return result;
        }
    }
}

class Program
{
    static void Main()
    {
        var cache = new FunctionCache<int, string>(TimeSpan.FromSeconds(10));

        string result1 = cache.GetOrAdd(1, key => "Value " + key);
        Console.WriteLine(result1);

        string result2 = cache.GetOrAdd(1, key => "New Value " + key);
        Console.WriteLine(result2);
    }
}

