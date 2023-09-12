
namespace Parsing.Expressions;

//left-right expression parser
public sealed class LRExpressionParser
{
    private readonly List<Expression> parseList;
    private readonly IEnumerator<Symbol> symbols;

    public LRExpressionParser(string s)
    {
        parseList = new();
        symbols = ReadSymbols(s).GetEnumerator();
    }

    public Expression Parse()
    {
        // keep stack of previous operator and next operator
        
        // if next symbol is an operator
        // and if two operators are not next to eachother
        // and (if the precedence of the previous operator is less than the next
        // or (if they are equal
        // and they are left-right operators)) : combine
        
        // otherwise : next

        throw new NotImplementedException();
    }

    private void Next()
    {
        parseList.Add(symbols.Current);
        symbols.MoveNext();
    }

    //parses the parse-list from right to left
    private void Combine()
    {
        for(int i = parseList.Count - 1; i >= 0; i--)
        {
            if(parseList[i] is not Symbol symbol)
            {
                continue;
            }

            switch(symbol.Name)
            {
                //infix binary operators
                case "&": case "|":
                    Expression leftOperand = parseList[i - 1];
                    Expression rightOperand = parseList[i + 1];
                    Operation operation = new(symbol, leftOperand, rightOperand);
                    parseList.RemoveRange(i - 1, 3);
                    parseList.Insert(i - 1, operation);
                    break;
                //prefix unary operators
                case "~":
                    Expression operand = parseList[i + 1];
                    Operation operation2 = new(symbol, operand);
                    parseList.RemoveRange(i, 2);
                    parseList.Insert(i, operation2);
                    break;
            }
        }
    }

    private static IEnumerable<Symbol> ReadSymbols(string s)
    {
        foreach(char c in s)
        {
            yield return new Symbol(c.ToString());
        }
    }
}
