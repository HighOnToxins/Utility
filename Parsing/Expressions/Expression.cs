
namespace Parsing.Expressions;

public abstract class Expression
{
    public Expression? Operator { get; internal set; }


}

public sealed class SymbolExp: Expression
{
    public string Symbol { get; private init; }


    public SymbolExp(string symbol) 
    {
        Symbol = symbol;
    }
}

public sealed class Operator : Expression
{
    public IReadOnlyList<Expression> Operands { get; private init; }


    public Operator(params Expression[] operands) : this((IReadOnlyList<Expression>) operands)
    {
    }

    public Operator(IReadOnlyList<Expression> operands)
    {
        Operands = operands;

        foreach(Expression expression in Operands)
        {
            if(expression.Operator != null)
            {
                throw new ArgumentException($"The given expression {expression} is allready an operand of another expression.");
            }
            expression.Operator = this;
        }
    }

}