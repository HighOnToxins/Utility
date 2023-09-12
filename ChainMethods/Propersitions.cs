using System.Diagnostics.CodeAnalysis;

namespace ChainMethods;

public static class Propositions
{
    public static bool TryGet<T>(Func<T?> getter, [NotNullWhen(true)] out T? result) 
    {
        result = getter.Invoke();
        return result is not null;
    }

}