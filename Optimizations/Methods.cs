
using System.Runtime.CompilerServices;

namespace Optimizations;

public static class Methods
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ulong ToUlong(this bool b) => *(ulong*)&b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe int ToInt(this bool b) => *(int*)&b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Abs(int a) => a * ((a >= 0).ToInt() * 2 - 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Abs(double a) => a * ((a >= 0).ToInt() * 2 - 1);


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Max(int a, int b) => Lerp(a > b, a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Min(int a, int b) => Lerp(a < b, a, b);



    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Lerp(float inter, int start, int end) => (int)((end - start) * inter) + start;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Lerp(bool inter, int start, int end) => (end - start) * inter.ToInt() + start;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Lerp(double inter, double start, double end) => (end - start) * inter + start;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double IntLerp(bool inter, double start, double end) => (end - start) * inter.ToInt() + start;

}
