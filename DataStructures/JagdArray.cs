
namespace DataStructures;

public class JagdArray<T> : SubArray<T>
{
    private readonly SubArray<T>[] array;

    public JagdArray(int rank, int size)
    {
        if(rank == 0)
        {
            array = new JArray<T>[size];
        }
        else if(rank == 1)
        {
            array = new JArray<T>[size];
        }
        else
        {
            array = new JagdArray<T>[size];
        }
    }


}

public class JArray<T>: SubArray<T>
{
    private readonly T[] array;

    public JArray(int size)
    {

    }
}

public interface SubArray<T>
{

}