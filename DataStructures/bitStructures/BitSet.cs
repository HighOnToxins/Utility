namespace DataStructures.bitStructures;

public class BitSet<T>
{
    private readonly UniversalBitSet<T> universe;

    private readonly BitList list;

    public BitSet(UniversalBitSet<T> universe)
    {
        this.universe = universe;
        list = new();
    }

    private BitSet(UniversalBitSet<T> universe, BitList list) : this(universe)
    {
        this.list = list;
    }

    public int Count => list.Count;

    public void Add(T value)
    {
        int index = universe.Add(value);
        list.Set(index);
    }

    public void Remove(T value)
    {
        int index = universe.IndexOf(value);
        if (index != -1) list.Set(index, false);
    }

    public bool Contains(T value)
    {
        int index = universe.IndexOf(value);
        return list.Get(index);
    }

    public BitSet<T> Union(BitSet<T> other)
    {
        return new BitSet<T>(universe, list.Or(other.list));
    }

    public BitSet<T> Intersect(BitSet<T> other)
    {
        return new BitSet<T>(universe, list.And(other.list));
    }

}

public class UniversalBitSet<T>
{

    private readonly List<T> values;

    public UniversalBitSet()
    {
        values = new();
    }

    //when something is added then it can't be removed
    public int Add(T value)
    {
        if (!values.Contains(value)) values.Add(value);
        return IndexOf(value);
    }

    public int IndexOf(T value) => values.IndexOf(value);

}
