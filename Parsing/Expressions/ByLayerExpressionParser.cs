
namespace Parsing.Expressions;

/// <summary> An expression parser that parses each layer one at a time. And uses parenthesis to inform structure.</summary>
public static class ByLayerExpressionParser
{
    public static Expression Parse(string s)
    {
        return ParseRelation(new string(s.Where(c => !char.IsWhiteSpace(c)).ToArray()));
    }

    private static Expression ParseRelation(string s)
    {
        List<Expression> operands = new();
        int lastIndex = 0;
        int layer = 0;
        for(int i = 0; i < s.Length; i++)
        {
            switch(s[i])
            {
                case '=': case '>': case '<':
                    if(layer == 0)
                    {
                        operands.Add(ParseOr(s[lastIndex..i]));
                        operands.Add(ParseSymbol(s[i].ToString()));
                        lastIndex = i + 1;
                    }
                    break;
                case '(': layer++; break;
                case ')': layer--; break;
            }
        }

        if(operands.Count > 0)
        {
            operands.Add(ParseOr(s[lastIndex..^0]));
            return new Operator(operands);
        }
        else
        {
            return ParseOr(s);
        }
    }

    private static Expression ParseOr(string s)
    {
        int layer = 0;
        for(int i = s.Length - 1; i >= 0; i--)
        {
            switch(s[i])
            {
                case '|':
                    if(layer == 0)
                    {
                        Expression leftOperand = ParseOr(s[..i]);
                        Expression @operator = ParseSymbol(s[i].ToString());
                        Expression rightOperand = ParseAnd(s[(i + 1)..]);
                        return new Operator(leftOperand, @operator, rightOperand);
                    }
                    break;
                case '(': layer++;  break;
                case ')': layer--;  break;
            }
        }
        
        return ParseAnd(s);
    }

    private static Expression ParseAnd(string s)
    {
        int layer = 0;
        for(int i = s.Length - 1; i >= 0; i--)
        {
            switch(s[i])
            {
                case '&':
                    if(layer == 0)
                    {
                        Expression leftOperand = ParseAnd(s[..i]);
                        Expression @operator = ParseSymbol(s[i].ToString());
                        Expression rightOperand = ParseNot(s[(i + 1)..]);
                        return new Operator(leftOperand, @operator, rightOperand);
                    }
                    break;
                case '(': layer--; break;
                case ')': layer++; break;
            }
        }

        return ParseNot(s);
    }

    private static Expression ParseNot(string s)
    {
        if(s[0] == '~')
        {
            Expression @operator = ParseSymbol(s[0].ToString());
            Expression operand = ParseNot(s[1..]);
            return new Operator(@operator, operand);
        }
        return ParseExpression(s);
    }

    private static Expression ParseExpression(string s)
    {
        int layer = 0;
        for(int i = s.Length - 1; i >= 0; i--)
        {
            switch(s[i])
            {
                case '+': case '-':
                    if(layer == 0 && i - 1 >= 0 && s[i - 1] != '+' && s[i - 1] != '-' && s[i - 1] != '*' && s[i - 1] != '/' && s[i - 1] != '%' && s[i - 1] != '^')
                    {
                        Expression leftOperand = ParseExpression(s[..i]);
                        Expression @operator = ParseSymbol(s[i].ToString());
                        Expression rightOperand = ParseSign(s[(i + 1)..]);
                        return new Operator(leftOperand, @operator, rightOperand);
                    }
                    break;
                case '(': layer--; break;
                case ')': layer++; break;
            }
        }
        return ParseTerm(s);
    }

    private static Expression ParseTerm(string s)
    {
        int layer = 0;
        for(int i = s.Length - 1; i >= 0; i--)
        {
            switch(s[i])
            {
                case '*': case '/': case '%':
                    if(layer == 0)
                    {
                        Expression leftOperand = ParseTerm(s[..i]);
                        Expression @operator = ParseSymbol(s[i].ToString());
                        Expression rightOperand = ParseSign(s[(i + 1)..]);
                        return new Operator(leftOperand, @operator, rightOperand);
                    }
                    break;
                case '(': layer--; break;
                case ')': layer++; break;
            }
        }
        return ParseSign(s);
    }

    private static Expression ParseSign(string s)
    {
        if(s[0] == '-' || s[0] == '+')
        {
            Expression operand = ParseSign(s[1..]);
            Expression @operator = ParseSymbol(s[0].ToString());
            return new Operator(@operator, operand);
        }
        return ParsePower(s);
    }

    private static Expression ParsePower(string s)
    {
        int layer = 0;
        for(int i = 0; i < s.Length; i++)
        {
            switch(s[i])
            {
                case '^':
                    if(layer == 0)
                    {
                        Expression leftOperand = ParseFactorial(s[..i]);
                        Expression @operator = ParseSymbol(s[i].ToString());
                        Expression rightOperand = ParseSign(s[(i + 1)..]);
                        return new Operator(leftOperand, @operator, rightOperand);
                    }
                    break;
                case '(': layer++; break;
                case ')': layer--; break;
            }
        }
        return ParseFactorial(s);
    }

    private static Expression ParseFactorial(string s)
    {
        if(s[^1] == '!')
        {
            Expression operand = ParseFactorial(s[..^1]);
            Expression @operator = ParseSymbol(s[^1].ToString());
            return new Operator(operand, @operator);
        }
        return ParseParameter(s);
    }

    private static Expression ParseParameter(string s)
    {
        int layer = 0;
        int i = s.Length;
        bool isNotDot = false;
        for(int j = 0; j < s.Length; j++)
        {
            if(layer == 0 && s[j] != '.')
            {
                if(isNotDot)
                {
                    i = j;
                    break;
                }
                isNotDot = true;
            }
            else
            {
                isNotDot = false;
            }

            switch(s[j])
            {
                case '(': layer++; break;
                case ')': layer--; break;
            }

        }

        Expression leftOperand = ParseDot(s[.. i]);

        if(i == s.Length)
        {
            return leftOperand;
        }

        Expression rightOperand = ParseParameter(s[i..]);
        return new Operator(leftOperand, rightOperand);
    }

    private static Expression ParseDot(string s)
    {
        int layer = 0;
        for(int i = s.Length - 1; i >= 0; i--)
        {
            switch(s[i])
            {
                case '.':
                    if(layer == 0)
                    {
                        Expression leftOperand = ParseDot(s[..i]);
                        Expression @operator = ParseSymbol(s[i].ToString());
                        Expression rightOperand = ParseContainer(s[(i + 1)..]);
                        return new Operator(leftOperand, @operator, rightOperand);
                    }
                    break;
                case '(': layer--; break;
                case ')': layer++; break;
            }
        }

        return ParseContainer(s);
    }

    private static Expression ParseContainer(string s)
    {
        if(s[0] == '(' && s[^1] == ')')
        {
            bool isTuple = false;
            List<Expression> operands = new() {
                ParseSymbol(s[0].ToString()),
            };

            int layer = 1;
            int lastIndex = 1;
            for(int j = 1; j < s.Length; j++)
            {
                switch(s[j])
                {
                    case ',':
                    case ';':
                        isTuple = true;
                        if(layer == 1)
                        {
                            if(s[lastIndex..j] != "")
                            {
                                operands.Add(Parse(s[lastIndex..j]));
                            }
                            operands.Add(ParseSymbol(s[j].ToString()));
                            lastIndex = j + 1;
                        }
                        break;
                    case '(': layer++; break;
                    case ')': layer--; break;
                }
            }

            if(s[lastIndex..^1] != "")
            {
                operands.Add(Parse(s[lastIndex..^1]));
            }

            if(isTuple || operands.Count == 1)
            {
                operands.Add(ParseSymbol(s[^1].ToString()));
                return new Operator(operands);
            }
            else
            {
                return operands[1];
            }

        }

        return ParseSymbol(s);
    }

    private static Expression ParseSymbol(string s)
    {
        return new SymbolExp(s);
    }
}