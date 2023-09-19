
using System.Collections;

namespace DataStructures.bitStructures;

public class BitSet<T> where T : notnull
{
    private readonly IBitUniverse<T> universe;

    private readonly BitArray array;

    public BitSet(IBitUniverse<T> universe)
    {
        this.universe = universe;
        array = new(universe.Length);
    }

    private BitSet(IBitUniverse<T> universe, BitArray array) : this(universe)
    {
        if(universe.Length != array.Length) throw new ArgumentException();

        this.array = array;
    }

    public void Add(T value) => array[universe.IndexOf(value)] = true;

    public void Remove(T value) => array[universe.IndexOf(value)] = false;

    public bool Contains(T value) => array[universe.IndexOf(value)];

    public BitSet<T> Union(BitSet<T> other) => new(universe, array.Or(other.array));

    public BitSet<T> Intersect(BitSet<T> other) => new(universe, array.And(other.array));

    public BitSet<T> Compliment() => new(universe, array.Not());
}

public interface IBitUniverse<T> where T : notnull
{
    public int Length { get; }

    public int IndexOf(T value);
}
