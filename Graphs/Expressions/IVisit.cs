
namespace Graphs.Expressions; 

public interface IVisit<T> where T : IExpression<T>{

    public void Visit(T expression);

}