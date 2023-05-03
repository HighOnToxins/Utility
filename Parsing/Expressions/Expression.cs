
namespace Parsing.Expressions;

public abstract class Expression
{
    public Expression? Parent { get; internal set; }

    public abstract Expression Clone();
}

public sealed class Symbol: Expression
{
    public string Name { get; private init; }


    public Symbol(string name) 
    {
        Name = name;
    }

    public override Expression Clone()
    {
        return new Symbol(Name);
    }
}

public sealed class Operation : Expression
{
    public Expression Operator { get; private init; }
    public IReadOnlyList<Expression> Operands { get; private init; }

    public Operation(Expression @operator, params Expression[] operands) : this(@operator, (IReadOnlyList<Expression>) operands)
    {
    }

    public Operation(Expression @operator, IReadOnlyList<Expression> operands)
    {
        Operator = @operator;
        Operands = operands;

        foreach(Expression expression in Operands)
        {
            if(expression.Parent != null)
            {
                throw new ArgumentException($"The given expression {expression} is allready an operand of another expression.");
            }
            expression.Parent = this;
        }
    }

    public override Expression Clone()
    {
        return new Operation(Operator.Clone(), Operands.Select(e => e.Clone()).ToArray());
    }
}