using Optimizations;

namespace DataStructures.bitStructures;

public class BitList
{
    private readonly List<ulong> values;

    private readonly int maxLength;

    public BitList(int maxLength = int.MaxValue)
    {
        this.maxLength = maxLength;
        values = new();
    }

    public BitList(int maxLength = int.MaxValue, List<ulong> values)
    {
        this.maxLength = maxLength;
        this.values = values;
    }

    public int Count;

    public void Set(int index, bool value = true)
    {
        if (index > maxLength) return;

        int listIndex = index / sizeof(ulong);
        int bitIndex = index % sizeof(ulong);

        while (values.Count < listIndex) values.Add(0ul);

        ulong mask = 1ul << bitIndex;
        values[listIndex] = values[listIndex] & ~mask | mask * value.ToUlong();
    }

    public bool Get(int index)
    {
        if (index > maxLength) return false;

        int listIndex = index / sizeof(ulong);

        if (values.Count >= listIndex) return false;

        int bitIndex = index % sizeof(ulong);
        return (values[listIndex] >> bitIndex & 1ul) != 0;
    }

    public BitList Or(BitList other)
    {
        int maxCount = Math.Max(other.Count, Count);
        List<ulong> newValues = new();
        for (int i = 0; i < maxCount; i++)
        {
            ulong thisBits = i < values.Count ? values[i] : 0ul;
            ulong otherBits = i < other.values.Count ? other.values[i] : 0ul;

            newValues.Add(thisBits | otherBits);
        }

        return new BitList(Math.Max(maxLength, other.maxLength), newValues);
    }

    public BitList And(BitList other)
    {
        throw new NotImplementedException();
    }
}
