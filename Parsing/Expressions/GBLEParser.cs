
namespace Parsing.Expressions;

//Generalized by-layer expression parser
public static class GBLEParser
{
    public static Expression Parse(string s)
    {
        string s2 = new(s.Where(c => !char.IsWhiteSpace(c)).ToArray());

        //ParseEquation 
        return ParseBinaryLeft(s2, s =>// Equation
            ParseBinaryLeft(s, s =>    // Or
            ParseBinaryLeft(s, s =>    // And
            ParseUnaryPrefix(s, s =>   // Not
            ParseBinaryLeft(s, s =>    // Relations
            ParseBinaryLeft(s, s =>    // Expression
            ParseBinaryLeft(s, s =>    // Term
            //ParseUnaryPrefix(s, s =>   // Sign
            ParseBinaryRight(s, s =>   // Power
            ParseUnaryPostfix(s, s =>  // Factorial
            ParseParameter(s, s =>     // Parameter
            ParseBinaryLeft(s, s =>    // Dot
            ParseParenthesis(s),       // Tuples/Parenthesis
                '.')), 
                '!'), 
                '^')
                /*, '+', '-')*/, 
                '*', '/', '%'), 
                '+', '-'), 
                '>', '<'), 
                '~'), 
                '&'), 
                '|'), 
                '=');

    }

    private static Expression ParseParameter(string s, Func<string, Expression> nxtParser)
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

        Expression leftOperand = nxtParser.Invoke(s[..i]);

        if(i == s.Length)
        {
            return leftOperand;
        }

        Expression rightOperand = ParseParameter(s[i..], nxtParser);
        return new Operation(leftOperand, rightOperand);
    }

    private static Expression ParseParenthesis(string s)
    {
        if(s[0] == '(' && s[^1] == ')')
        {
            bool isTuple = false;
            List<Expression> operands = new()
            {
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
                Expression @operator = new Symbol(s[0].ToString());
                return new Operation(@operator, operands);
            }
            else
            {
                return operands[0];
            }

        }

        return new Symbol(s);
    }

    private static Expression ParseBinaryLeft(string s, Func<string, Expression> nxtParser, params char[] operators)
    {
        int layer = 0;
        for(int i = s.Length - 1; i >= 0; i--)
        {
            if(operators.Contains(s[i]))
            {
                Expression @operator = new Symbol(s[i].ToString());
                Expression leftOperand = ParseBinaryLeft(s[..i], nxtParser, operators);
                Expression rightOperand = nxtParser.Invoke(s[(i + 1)..]);
                return new Operation(@operator, leftOperand, rightOperand);
            }

            switch(s[i])
            {
                case '(': layer--; break;
                case ')': layer++; break;
            }
        }

        return nxtParser.Invoke(s);
    }

    private static Expression ParseBinaryRight(string s, Func<string, Expression> nxtParser, params char[] operators)
    {
        int layer = 0;
        for(int i = 0; i < s.Length; i++)
        {
            if(operators.Contains(s[i]))
            {
                Expression @operator = new Symbol(s[i].ToString());
                Expression leftOperand = nxtParser.Invoke(s[..i]);
                Expression rightOperand = ParseBinaryRight(s[(i + 1)..], nxtParser, operators);
                return new Operation(@operator, leftOperand, rightOperand);
            }

            switch(s[i])
            {
                case '(': layer++; break;
                case ')': layer--; break;
            }
        }

        return nxtParser.Invoke(s);
    }

    private static Expression ParseUnaryPrefix(string s, Func<string, Expression> nxtParser, params char[] operators)
    {
        if(operators.Contains(s[0]))
        {
            Expression @operator = new Symbol(s[0].ToString());
            Expression operand = ParseUnaryPrefix(s, nxtParser, operators);
            return new Operation(@operator, operand);
        }

        return nxtParser.Invoke(s);
    }

    private static Expression ParseUnaryPostfix(string s, Func<string, Expression> nxtParser, params char[] operators)
    {
        if(operators.Contains(s[^1]))
        {
            Expression @operator = new Symbol(s[^1].ToString());
            Expression operand = ParseUnaryPostfix(s[..^1], nxtParser, operators);
            return new Operation(@operator, operand);
        }

        return nxtParser.Invoke(s[..]);
    }

}