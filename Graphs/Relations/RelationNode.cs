
namespace Graphs.Relations;

public sealed class RelationNode<T>: IRelationNode<T>, IReadOnlyRelationNode<T> where T : notnull{

    public T Value { get; set; }

    public Relation<T>? Relation { get; }

    internal List<RelationNode<T>> InternalRelations { get; private init; }
        
    public IReadOnlyList<RelationNode<T>> Relations { get => InternalRelations;}

    IRelation<T>? IRelationNode<T>.Relation => Relation;

    IReadOnlyRelation<T>? IReadOnlyRelationNode<T>.Relation => Relation;

    public IEnumerable<IRelationNode<T>> GetRelations() => InternalRelations;

    IEnumerable<IReadOnlyRelationNode<T>> IReadOnlyRelationNode<T>.GetRelations() => InternalRelations;

    public RelationNode(T value) {
        Value = value;
        InternalRelations = new();
    }

}