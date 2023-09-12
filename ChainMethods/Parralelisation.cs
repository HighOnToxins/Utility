
namespace DataStructures;

public static class Parallelization
{
    public static IEnumerable<(T1?, T2?)> MergeWith<T1, T2>(this IEnumerable<T1> e1, IEnumerable<T2> e2)
    {
        IEnumerator<T1> er1 = e1.GetEnumerator();
        IEnumerator<T2> er2 = e2.GetEnumerator();
        
        bool mn1 = true;
        bool mn2 = true;

        while(mn1 || mn2)
        {
            mn1 = er1.MoveNext();
            mn2 = er2.MoveNext();

            yield return (mn1 ? er1.Current : default, mn2 ? er2.Current : default);
        }
    }
}
