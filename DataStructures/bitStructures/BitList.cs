using Optimizations;

namespace DataStructures.bitStructures;

public class BitList
{
    private readonly List<ulong> values;

    public BitList()
    {
        values = new();
    }

    private BitList(int count, List<ulong> values)
    {
        Count = count;
        this.values = values;
    }

    public int Count;

    public void Add(bool value)
    {
        int bitIndex = Count % sizeof(ulong);
        if(bitIndex == 0) values.Add(0ul);
        values[^1] = value.ToUlong() << bitIndex;
    }

    public void RemoveAt(int index)
    {
        throw new NotImplementedException();
    }

    public bool Get(int index)
    {
        int listIndex = index / sizeof(ulong);
        int bitIndex = index % sizeof(ulong);
        return ((values[listIndex] >> bitIndex) & 1ul) != 0;
    }

    public BitList Operate(BitList other, Func<ulong, ulong, ulong> operation)
    {
        int maxCount = Math.Max(other.Count, Count);
        List<ulong> newValues = new();
        for (int i = 0; i < maxCount; i++)
        {
            ulong thisBits = i < values.Count ? values[i] : 0ul;
            ulong otherBits = i < other.values.Count ? other.values[i] : 0ul;

            newValues.Add(operation.Invoke(thisBits, otherBits));
        }

        return new BitList(maxCount, newValues);
    }

    public BitList Or(BitList other) => Operate(other, (a, b) => a | b);

    public BitList And(BitList other) => Operate(other, (a, b) => a & b);
}
