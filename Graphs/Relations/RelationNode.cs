
namespace Graphs.Relations;

public sealed class RelationNode<T> where T : notnull{

    public T Value { get; set; }

    public Relation<T>? Relation { get; }

    internal List<RelationNode<T>> InternalRelations { get; private init; }
        
    public IReadOnlyList<RelationNode<T>> Relations { get => InternalRelations;}

    public RelationNode(T value) {
        Value = value;
        InternalRelations = new();
    }

}