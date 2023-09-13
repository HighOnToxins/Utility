using Optimizations;
using System;

namespace DataStructures.bitStructures;

public class BitArray64
{
    private readonly ulong[] values;

    public BitArray64(int length)
    {
        Length = length;
        values = new ulong[length / sizeof(ulong)];
    }

    private BitArray64(int length, ulong[] values)
    {
        Length = length;
        this.values = values;
    }

    public int Length;

    public bool this[int index]
    {
        get => Get(index);
        set => Set(index, value);
    }

    public bool Get(int index)
    {
        int listIndex = index / sizeof(ulong);
        int bitIndex = index % sizeof(ulong);
        return (values[listIndex] >> bitIndex & 1ul) != 0;
    }

    public void Set(int index, bool value = true)
    {
        int listIndex = index / sizeof(ulong);
        int bitIndex = index % sizeof(ulong);

        ulong mask = 1ul << bitIndex;
        values[listIndex] = (values[listIndex] & ~mask) | (mask * value.ToUlong());
    }

    private BitArray64 OperateBinary(BitArray64 other, Func<ulong, ulong, ulong> operation)
    {
        int maxLength = Math.Max(other.Length, Length);
        ulong[] newValues = new ulong[maxLength];
        for(int i = 0; i < maxLength; i++)
        {
            ulong thisBits = i < values.Length ? values[i] : 0ul;
            ulong otherBits = i < other.values.Length ? other.values[i] : 0ul;

            newValues[i] = operation.Invoke(thisBits, otherBits);
        }

        return new BitArray64(Math.Max(Length, other.Length), newValues);
    }

    private BitArray64 OperateUnary(Func<ulong, ulong> operation)
    {
        ulong[] newValues = new ulong[Length];
        for(int i = 0; i < Length; i++)
        {
            newValues[i] = operation.Invoke(values[i]);
        }

        return new BitArray64(Length, newValues);
    }

    public BitArray64 And(BitArray64 other) => OperateBinary(other, (a, b) => a & b);

    public BitArray64 Or(BitArray64 other) => OperateBinary(other, (a, b) => a | b);

    public BitArray64 Not() => OperateUnary(a => ~a);

    public static BitArray64 operator &(BitArray64 a, BitArray64 b) => a.And(b);
    public static BitArray64 operator |(BitArray64 a, BitArray64 b) => a.Or(b);
    public static BitArray64 operator ~(BitArray64 a) => a.Not();


}
