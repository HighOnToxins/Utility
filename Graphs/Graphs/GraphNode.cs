
namespace Graphs.Relations;

public sealed class GraphNode<T> where T : notnull{

    public T Value { get; set; }

    public Graph<T>? Relation { get; }

    private readonly Dictionary<T, GraphNode<T>> relationsTo;
    private readonly Dictionary<T, GraphNode<T>> relationsFrom;

    public IReadOnlyDictionary<T, GraphNode<T>> RelationsTo { get => relationsTo; }
    public IReadOnlyDictionary<T, GraphNode<T>> RelationsFrom { get => relationsFrom; }

    public GraphNode(T value) {
        Value = value;
        relationsTo = new();
        relationsFrom = new();
    }

    internal void AddEdgeTo(GraphNode<T> node) {
        relationsTo.Add(node.Value, node);
        node.relationsFrom.Add(Value, this);
    }

    internal void CutFrom(GraphNode<T> node) {
        relationsTo.Remove(node.Value);
    }

    internal void RemoveEdgeTo(GraphNode<T> node)  {
        relationsTo.Remove(node.Value);
    }
}