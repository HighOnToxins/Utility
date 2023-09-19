
using System.Collections;

namespace DataStructures.bitStructures;

public class BitSet<T> : IEnumerable<T> where T : notnull
{
    private readonly IBitUniverse<T> universe;

    private readonly BitArray64 array;

    public BitSet(IBitUniverse<T> universe)
    {
        this.universe = universe;
        array = new(universe.Length);
    }

    private BitSet(IBitUniverse<T> universe, BitArray64 array) : this(universe)
    {
        if(universe.Length != array.Length) throw new ArgumentException();

        this.array = array;
    }

    public void Add(T value) => array[universe.IndexOf(value)] = true;

    public void Remove(T value) => array[universe.IndexOf(value)] = false;

    public bool Contains(T value) => array[universe.IndexOf(value)];

    public BitSet<T> Union(BitSet<T> other) => new(universe, array | other.array);

    public BitSet<T> Intersect(BitSet<T> other) => new(universe, array & other.array);

    public BitSet<T> Compliment() => new(universe, ~array);

    public HashSet<T> ToHashSet() => new(
            
        );

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => universe
                .Values()
                .Where(v => Contains(v)).GetEnumerator();
    public IEnumerator GetEnumerator() => universe
                .Values()
                .Where(v => Contains(v)).GetEnumerator();

}

public interface IBitUniverse<T> where T : notnull
{
    public int Length { get; }
    public int IndexOf(T value);

    public IEnumerable<T> Values();
}
