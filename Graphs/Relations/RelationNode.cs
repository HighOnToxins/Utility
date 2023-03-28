
namespace Graphs.Relations;

public sealed class RelationNode<T> where T : notnull{

    public T Value { get; set; }

    public Relation<T>? Relation { get; private set; }

    private readonly Dictionary<T, RelationNode<T>> outwardEdges;
    private readonly Dictionary<T, RelationNode<T>> inwardEdges;

    public IReadOnlyDictionary<T, RelationNode<T>> OutwardEdges { get => outwardEdges; }
    public IReadOnlyDictionary<T, RelationNode<T>> InwardEdges { get => inwardEdges; }

    public RelationNode(T value) {
        Value = value;
        outwardEdges = new();
        inwardEdges = new();
    }

    internal RelationNode(T value, Relation<T> relation) : this(value){
        Relation = relation;
    }

    internal bool AddEdgeTo(RelationNode<T> node) {
        if(outwardEdges.ContainsKey(node.Value)) {
            return false;
        }
        
        outwardEdges.Add(node.Value, node);
        node.inwardEdges.Add(Value, this);
        return true;
    }

    internal bool RemoveEdgeTo(RelationNode<T> node)  {
        bool b0 = outwardEdges.Remove(node.Value);
        bool b1 = node.inwardEdges.Remove(Value);
        return b0 || b1;
    }
}