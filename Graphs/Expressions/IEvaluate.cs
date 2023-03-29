
namespace Graphs.Expressions;

public interface IEvaluate<T> where T : IExpression<T> {

    public T Evaluate(T expression);

}