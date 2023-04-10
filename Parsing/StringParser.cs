namespace Parsing;

public static class StringParser
{

    public static IEnumerable<string[]> RightReadAll(string format, string s)
    {
        string[] operators = format.Split("%s");
        MatchEnds(ref operators, ref s);
        return RecursiveRightReadAll(operators, operators.Length - 1, s, s.Length - 1);
    }

    private static IEnumerable<string[]> RecursiveRightReadAll(string[] operators, int operatorIndex, string s, int stringStartIndex)
    {
        if(operatorIndex < 0)
        {
            string[] outputs = new string[operators.Length + 1];
            outputs[0] = s[..(stringStartIndex + 1)];
            yield return outputs;
            yield break;
        }

        int stringIndex = stringStartIndex;
        while(stringIndex >= 0)
        {
            int indexOfNextFormat = s.LastIndexOf(operators[operatorIndex], stringIndex);
            if(indexOfNextFormat <= -1) break;

            IEnumerable<string[]> inputs = RecursiveRightReadAll(operators, operatorIndex - 1, s, indexOfNextFormat - operators[operatorIndex].Length);
            string str = s[(indexOfNextFormat + 1)..(stringStartIndex + 1)];

            foreach(string[] input in inputs)
            {
                input[operatorIndex + 1] = str;
                yield return input;
            }

            stringIndex = indexOfNextFormat - 1;
        }
    }

    public static IEnumerable<string[]> LeftReadAll(string format, string s)
    {
        string[] formatter = format.Split("%s");
        MatchEnds(ref formatter, ref s);
        return RecursiveLeftReadAll(formatter, 0, s, 0);
    }

    private static IEnumerable<string[]> RecursiveLeftReadAll(string[] operators, int operatorIndex, string s, int stringStartIndex)
    {
        if(operatorIndex >= operators.Length)
        {
            string[] outputs = new string[operators.Length + 1];
            outputs[^1] = s[stringStartIndex..];
            yield return outputs;
            yield break;
        }

        int stringIndex = stringStartIndex;
        while(stringIndex < s.Length)
        {
            int indexOfNextFormat = s.IndexOf(operators[operatorIndex], stringIndex);
            if(indexOfNextFormat <= -1) break;

            IEnumerable<string[]> inputs = RecursiveLeftReadAll(operators, operatorIndex + 1, s, indexOfNextFormat + operators[operatorIndex].Length);
            string str = s[stringStartIndex..indexOfNextFormat];

            foreach(string[] input in inputs)
            {
                input[operatorIndex] = str;
                yield return input;
            }

            stringIndex = indexOfNextFormat + 1;
        }
    }

    public static string[] LeftRead(string format, string s)
    {

        string[] operators = format.Split("%s");
        MatchEnds(ref operators, ref s);

        int stringIndex = 0;
        string[] strings = new string[operators.Length + 1];
        for(int i = 0; i < operators.Length; i++)
        {
            int indexOfNextFormat = s.IndexOf(operators[i], stringIndex);
            if(indexOfNextFormat <= -1) throw new ArgumentException($"The string did not match the given format at index {stringIndex}.");

            strings[i] = s[stringIndex..indexOfNextFormat];
            stringIndex = indexOfNextFormat + operators[i].Length;
        }

        strings[^1] = s[stringIndex..];

        return strings;
    }

    public static string[] RightRead(string format, string s)
    {

        string[] operators = format.Split("%s");
        MatchEnds(ref operators, ref s);

        int stringIndex = s.Length - 1;
        string[] stringInputs = new string[operators.Length + 1];
        for(int i = operators.Length - 1; i >= 0; i--)
        {
            int indexOfNextFormat = s.LastIndexOf(operators[i], stringIndex);
            if(indexOfNextFormat <= -1) throw new ArgumentException($"The string did not match the given format at index {stringIndex}.");

            stringInputs[i + 1] = s[(indexOfNextFormat + 1)..(stringIndex + 1)];
            stringIndex = indexOfNextFormat - operators[i].Length;
        }

        stringInputs[0] = s[..(stringIndex + 1)];

        return stringInputs;
    }

    private static void MatchEnds(ref string[] formatter, ref string s)
    {

        if(!s.StartsWith(formatter[0]))
        {
            throw new ArgumentException($"The string did not match the given format at index {0}.");
        }

        if(!s.EndsWith(formatter[^1]))
        {
            throw new ArgumentException($"The string did not match the given format at index {s.Length - 1}.");
        }

        s = s[formatter[0].Length..^formatter[^1].Length];
        formatter = formatter[1..^1];

    }
}