
namespace Graphs.Relations;

public interface IReadOnlyRelation<T>: IEnumerable<IRelationNode<T>> {

    public int Count { get; }
    public int RelationCount { get; }

}