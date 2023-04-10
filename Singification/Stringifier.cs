
namespace Singification;

using System.Collections;
using System.Reflection;

public static class Stringifier
{

    public static string Stringify(this object? obj, int maxDepth = int.MaxValue)
    {
        if(obj == null)
        {
            return "null";
        }

        switch(obj)
        {
            case IConvertible convertible:
                return convertible.ToString() ?? "null";

            case Array array:
                if(maxDepth <= 0)
                {
                    return "_";
                }

                List<string> elements = new();
                foreach(object? x in array)
                {
                    elements.Add(x.Stringify(maxDepth));
                };
                return $"[{string.Join(", ", elements)}]";

            case IEnumerable enumerable:
                if(maxDepth <= 0)
                {
                    return "_";
                }

                List<string> elements2 = new();
                foreach(object? o in enumerable)
                {
                    elements2.Add(o.Stringify(maxDepth));
                }
                return $"{obj.GetType().Name}{{{string.Join(", ", elements2)}}}";

            default:
                if(maxDepth <= 0)
                {
                    return "_";
                }

                FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                List<string> elements3 = new();
                foreach(FieldInfo field in fields)
                {
                    elements3.Add($"{field.Name} = {field.GetValue(obj).Stringify(maxDepth - 1)}");
                }

                return $"{obj.GetType().Name}{{{string.Join(", ", elements3)}}}";
        }
    }


    public static string TabedStringify(this object? obj, int maxDepth = int.MaxValue)
    {
        return TabedStringify(obj, 0, maxDepth);
    }

    public static string TabedStringify(this object? obj, uint indentCount, int maxDepth = int.MaxValue)
    {

        switch(obj)
        {
            case null:
                return "null";
            case IConvertible convertible:
                return (convertible.ToString() ?? "null");

            case Array array:
                if(maxDepth <= 0)
                {
                    return "_";
                }

                List<string> elements = new();
                foreach(object? x in array)
                {
                    elements.Add(Tabs(indentCount + 1) + x.TabedStringify(indentCount + 1, maxDepth));
                };
                return $"[\n{string.Join(", \n", elements)}\n{Tabs(indentCount)}]";

            case IEnumerable enumerable:
                if(maxDepth <= 0)
                {
                    return "_";
                }

                List<string> elements2 = new();
                foreach(object? o in enumerable)
                {
                    elements2.Add(Tabs(indentCount + 1) + o.TabedStringify(indentCount + 1, maxDepth));
                }
                return $"{obj.GetType().Name}{{\n{string.Join(", \n", elements2)}\n{Tabs(indentCount)}}}";

            default:
                if(maxDepth <= 0)
                {
                    return "_";
                }

                FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                List<string> elements3 = new();
                foreach(FieldInfo field in fields)
                {
                    elements3.Add($"{Tabs(indentCount + 1)}{field.Name} = {field.GetValue(obj).TabedStringify(indentCount + 1, maxDepth - 1)}");
                }

                return $"{obj.GetType().Name} {{\n{string.Join(", \n", elements3)}\n{Tabs(indentCount)}}}";
        }
    }

    private static string Tabs(uint amount)
    {
        string tabs = "";
        while(amount > 0)
        {
            tabs += "    ";
            amount--;
        }
        return tabs;
    }
}
