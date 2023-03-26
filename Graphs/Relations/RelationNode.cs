
namespace Graphs.Relations;

public sealed class RelationNode<T> where T : notnull{

    public T Value { get; set; }

    public Graph<T>? Relation { get; }

    private readonly Dictionary<T, RelationNode<T>> relationsTo;
    private readonly Dictionary<T, RelationNode<T>> relationsFrom;

    public IReadOnlyDictionary<T, RelationNode<T>> RelationsTo { get => relationsTo; }
    public IReadOnlyDictionary<T, RelationNode<T>> RelationsFrom { get => relationsFrom; }

    public RelationNode(T value) {
        Value = value;
        relationsTo = new();
        relationsFrom = new();
    }

    internal void AddEdgeTo(RelationNode<T> node) {
        relationsTo.Add(node.Value, node);
        node.relationsFrom.Add(Value, this);
    }

    internal void CutFrom(RelationNode<T> node) {
        relationsTo.Remove(node.Value);
    }

    internal void RemoveEdgeTo(RelationNode<T> node)  {
        relationsTo.Remove(node.Value);
    }
}