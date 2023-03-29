
namespace Graphs.Expressions;

public interface IExpression<T> where T : IExpression<T> {

    public T? Parent { get; }

    public IEnumerable<T> GetChildren();

}
