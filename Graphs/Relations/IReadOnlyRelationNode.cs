namespace Graphs.Relations;

public interface IReadOnlyRelationNode<T> {

    public IReadOnlyRelation<T>? Relation { get; }

    public IEnumerable<IReadOnlyRelationNode<T>> GetRelations();

}