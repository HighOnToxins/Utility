
namespace Optimizations;

public abstract class Cache<TResult>
{

    private TResult? _value;

    protected abstract TResult Assign();

    public TResult Get()
    {
        _value ??= Assign();
        return _value;
    }

    public bool IsCached()
    {
        return _value != null;
    }
}

public abstract class Cache<T, TResult> where T : notnull
{

    private readonly Dictionary<T, TResult> _values;

    public Cache()
    {
        _values = new Dictionary<T, TResult>();
    }

    protected abstract TResult Assign(T arg);

    public TResult Get(T arg)
    {
        if(!_values.TryGetValue(arg, out TResult? value) || value == null)
        {
            value = Assign(arg);
            _values.Add(arg, value);
        }

        return value;
    }

    public bool IsCached(T arg)
    {
        return _values.ContainsKey(arg);
    }
}

public sealed class FuncCache<T, TResult>: Cache<T, TResult> where T : notnull
{
    private readonly Func<T, TResult> _assign;

    public FuncCache(Func<T, TResult> assign) : base()
    {
        _assign = assign;
    }

    protected override TResult Assign(T arg)
    {
        return _assign.Invoke(arg);
    }
}