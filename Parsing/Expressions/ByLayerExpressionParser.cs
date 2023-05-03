
using System.Diagnostics.CodeAnalysis;

namespace Parsing.Expressions;

/// <summary> An expression parser that parses each layer one at a time. And uses parenthesis to inform structure.</summary>
public static class ByLayerExpressionParser
{
    public static Expression Parse(string s)
    {
        string s2 = new(s.Where(c => !char.IsWhiteSpace(c)).ToArray());
        return ParseEquation(s2);
    }

    private static Expression ParseEquation(string s)
    {
        int layer = 0;
        for(int i = s.Length - 1; i >= 0; i--)
        {
            switch(s[i])
            {
                case '=':
                    if(layer == 0)
                    {
                        Expression @operator = ParseSymbol(s[i].ToString());
                        Expression leftOperand = ParseExpression(s[..i]);
                        Expression rightOperand = ParseTerm(s[(i + 1)..]);
                        return new Operation(@operator, leftOperand, rightOperand);
                    }
                    break;
                case '(': layer--; break;
                case ')': layer++; break;
            }
        }
        return ParseOr(s);
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
                        Expression @operator = ParseSymbol(s[i].ToString());
                        Expression leftOperand = ParseOr(s[..i]);
                        Expression rightOperand = ParseAnd(s[(i + 1)..]);
                        return new Operation(@operator, leftOperand, rightOperand);
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
                        Expression @operator = ParseSymbol(s[i].ToString());
                        Expression leftOperand = ParseAnd(s[..i]);
                        Expression rightOperand = ParseNot(s[(i + 1)..]);
                        return new Operation(@operator, leftOperand, rightOperand);
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
            return new Operation(@operator, operand);
        }
        return ParseRelations(s);
    }

    private static Expression ParseRelations(string s)
    {
        int layer = 0;
        for(int i = s.Length - 1; i >= 0; i--)
        {
            switch(s[i])
            {
                case '>':
                case '<':
                    if(layer == 0)
                    {
                        Expression @operator = ParseSymbol(s[i].ToString());
                        Expression leftOperand = ParseRelations(s[..i]);
                        Expression rightOperand = ParseExpression(s[(i + 1)..]);
                        return new Operation(@operator, leftOperand, rightOperand);
                    }
                    break;
                case '(': layer--; break;
                case ')': layer++; break;
            }
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
                    {   //Expression analysis???
                        Expression leftOperand = ParseExpression(s[..i]);
                        Expression @operator = ParseSymbol(s[i].ToString());
                        Expression rightOperand = ParseTerm(s[(i + 1)..]);
                        return new Operation(@operator, leftOperand, rightOperand);
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
                        return new Operation(@operator, leftOperand, rightOperand);
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
        if((s[0] == '-' || s[0] == '+') && s.Length > 1)
        {
            Expression operand = ParseSign(s[1..]);
            Expression @operator = ParseSymbol(s[0].ToString());
            return new Operation(@operator, operand);
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
                        Expression @operator = ParseSymbol(s[i].ToString());
                        Expression leftOperand = ParseFactorial(s[..i]);
                        Expression rightOperand = ParseSign(s[(i + 1)..]); //Precedence goes up here instead of staying put (if above is unary?)
                        return new Operation(@operator, leftOperand, rightOperand);
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
            Expression @operator = ParseSymbol(s[^1].ToString());
            Expression operand = ParseFactorial(s[..^1]);
            return new Operation(@operator, operand);
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
        return new Operation(leftOperand, rightOperand);
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
                        Expression @operator = ParseSymbol(s[i].ToString());
                        Expression leftOperand = ParseDot(s[..i]);
                        Expression rightOperand = ParseContainer(s[(i + 1)..]);
                        return new Operation(@operator, leftOperand, rightOperand);
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

            if(isTuple || operands.Count == 0)
            {
                Expression @operator = ParseSymbol(s[0].ToString());
                return new Operation(@operator, operands);
            }
            else
            {
                return operands[0];
            }

        }

        return ParseSymbol(s);
    }

    private static Expression ParseSymbol(string s)
    {
        return new Symbol(s);
    }
}