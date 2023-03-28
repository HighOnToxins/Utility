
namespace Graphs.Relations;

public sealed class GraphNode<T> where T : notnull{

    public T Value { get; set; }

    public Graph<T>? Graph { get; private set; }

    private readonly Dictionary<T, GraphNode<T>> outwardEdges;
    private readonly Dictionary<T, GraphNode<T>> inwardEdges;

    public IReadOnlyDictionary<T, GraphNode<T>> OutwardEdges { get => outwardEdges; }
    public IReadOnlyDictionary<T, GraphNode<T>> InwardEdges { get => inwardEdges; }

    public GraphNode(T value) {
        Value = value;
        outwardEdges = new();
        inwardEdges = new();
    }

    internal GraphNode(T value, Graph<T> graph) : this(value){
        Graph = graph;
    }

    internal bool AddEdgeTo(GraphNode<T> node) {
        if(outwardEdges.ContainsKey(node.Value)) {
            return false;
        }

        outwardEdges.Add(node.Value, node);
        node.inwardEdges.Add(Value, this);
        return true;
    }

    internal bool CutFrom(GraphNode<T> node) {
        bool b0 = outwardEdges.Remove(node.Value);
        bool b1 = inwardEdges.Remove(node.Value);
        return b0 || b1;
    }

    internal bool RemoveEdgeTo(GraphNode<T> node)  {
        bool b0 = outwardEdges.Remove(node.Value);
        bool b1 = node.inwardEdges.Remove(Value);
        return b0 || b1;
    }
}