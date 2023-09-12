using Optimizations;

namespace DataStructures.bitStructures;

public class BitArray
{
    private readonly ulong[] values;

    public BitArray(int length)
    {
        Length = length;
        values = new ulong[length / sizeof(ulong)];
    }

    public BitArray(int length, ulong[] values)
    {
        Length = length;
        this.values = values;
    }

    public int Length;

    public void Set(int index, bool value = true)
    {
        int listIndex = index / sizeof(ulong);
        int bitIndex = index % sizeof(ulong);

        ulong mask = 1ul << bitIndex;
        values[listIndex] = values[listIndex] & ~mask | mask * value.ToUlong();
    }

    public bool Get(int index)
    {
        int listIndex = index / sizeof(ulong);

        int bitIndex = index % sizeof(ulong);
        return (values[listIndex] >> bitIndex & 1ul) != 0;
    }

    public BitArray Operate(BitArray other, Func<ulong, ulong, ulong> operation)
    {
        int maxLength = Math.Max(other.Length, Length);
        ulong[] newValues = new ulong[maxLength];
        for(int i = 0; i < maxLength; i++)
        {
            ulong thisBits = i < values.Length ? values[i] : 0ul;
            ulong otherBits = i < other.values.Length ? other.values[i] : 0ul;

            newValues[i] = operation.Invoke(thisBits, otherBits);
        }

        return new BitArray(Math.Max(Length, other.Length), newValues);
    }

    public BitArray Or(BitArray other) => Operate(other, (a, b) => a | b);

    public BitArray And(BitArray other) => Operate(other, (a, b) => a & b);
}
