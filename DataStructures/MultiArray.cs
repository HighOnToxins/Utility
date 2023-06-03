
namespace DataStructures;

public class MultiArray<T>
{
    private readonly int[] lengths;
    private readonly int[] stepSizes;

    private readonly T[] array;

    public MultiArray(params int[] lengths)
    {
        this.lengths = lengths;
        stepSizes = CompStepSizes(lengths);
        array = new T[stepSizes[^1] * lengths[^1]];
    }

    private static int[] CompStepSizes(int[] lengths)
    {
        if(lengths.Length == 0)
        {
            return Array.Empty<int>();
        }

        int[] stepSizes = new int[lengths.Length - 1];
        stepSizes[0] = lengths[0];

        for(int i = 1; i < lengths.Length; i += 2)
        {
            stepSizes[i] = stepSizes[i - 1] * lengths[i];
        }
        return stepSizes;
    }

    public int Rank => lengths.Length;

    public int GetLength(int dimension)
    {
        return lengths[dimension];
    }

    public T this[params int[] indecies] {
        get => array[GetIndex(indecies)];
        set => array[GetIndex(indecies)] = value;
    }

    private int GetIndex(int[] indecies)
    {
        if(lengths.Length != indecies.Length || lengths.Length == 0)
        {
            throw new IndexOutOfRangeException();
        }

        int index = indecies[0];
        for(int i = 1; i < indecies.Length; i++)
        {
            index += indecies[i] * stepSizes[i - 1];
        }

        return index;
    }
}