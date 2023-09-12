
namespace DataStructures.bitStructures;

public class BitSet<T>
{
    private readonly BitUniverse<T> universe;

    private readonly List<ulong> list;

    public BitSet(BitUniverse<T> universe)
    {
        this.universe = universe;
        list = new List<ulong>();
    }

    private BitSet(BitUniverse<T> universe, List<ulong> list) : this(universe)
    {
        this.list = list;
    }

    public void Add(T value)
    {
        int index = universe.Add(value);

        int listIndex = index / sizeof(ulong);
        while(listIndex >= list.Count) list.Add(0ul);
        
        int bitIndex = index % sizeof(ulong);
        list[listIndex] |= 1ul << bitIndex;
    }

    public void Remove(T value)
    {
        int index = universe.Add(value);

        int listIndex = index / sizeof(ulong);
        if(listIndex >= list.Count) return;

        int bitIndex = index % sizeof(ulong);
        list[listIndex] &= ~(1ul << bitIndex);
    }

    public bool Contains(T value)
    {
        int index = universe.Add(value);

        int listIndex = index / sizeof(ulong);
        if(listIndex >= list.Count) return false;
        
        int bitIndex = index % sizeof(ulong);
        return ((list[listIndex] >> bitIndex) & 1ul) != 0;
    }

    private BitSet<T> Operate(BitSet<T> other, Func<ulong, ulong, ulong> operation)
    {
        //alternativly create a new combined universe?
        if(!universe.Equals(other.universe))
        {
            throw new ArgumentException("the universes did not match!");
        }

        int maxCount = Math.Max(other.list.Count, list.Count);
        List<ulong> newValues = new();
        for(int i = 0; i < maxCount; i++)
        {
            ulong thisBits = i < list.Count ? list[i] : 0ul;
            ulong otherBits = i < other.list.Count ? other.list[i] : 0ul;

            newValues.Add(operation.Invoke(thisBits, otherBits));
        }

        return new BitSet<T>(universe, newValues);
    }

    public BitSet<T> Union(BitSet<T> other) => Operate(other, (a, b) => a | b);

    public BitSet<T> Intersect(BitSet<T> other) => Operate(other, (a, b) => a & b);

}

public class BitUniverse<T>
{

    private readonly List<T> values;

    public BitUniverse()
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
