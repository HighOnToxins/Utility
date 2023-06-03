
using System.Collections;

namespace Optimizations;

public sealed class EnumerableCache<T>: IEnumerable<T>
{
    private readonly List<T> _cache;
    private readonly IEnumerator<T> _enumerator;

    internal EnumerableCache(IEnumerable<T> enumerable)
    {
        _cache = new();
        _enumerator = enumerable.GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach(T item in _cache)
        {
            yield return item;
        }

        while(_enumerator.MoveNext())
        {
            _cache.Add(_enumerator.Current);
            yield return _enumerator.Current;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public static class LinkExtensions
{
    public static EnumerableCache<T> ToCache<T>(this IEnumerable<T> enumerable)
    {
        return new EnumerableCache<T>(enumerable);
    } 
}