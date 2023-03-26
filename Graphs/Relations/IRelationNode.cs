namespace Graphs.Relations;

public interface IRelationNode<T> {

    public IRelation<T>? Relation { get; }

    public IEnumerable<IRelationNode<T>> GetRelations();

}