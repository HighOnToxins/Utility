
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Intrinsics.X86;

namespace Graphs.Relations;

public class Graph<T>: IEnumerable<GraphNode<T>> where T : notnull {

    private readonly Dictionary<T, GraphNode<T>> nodes;

    public int NodeCount => nodes.Count;

    public int EdgeCount { get; private set; }

    public Graph() {
        nodes = new();
        EdgeCount = 0;
    }

    public bool AddVertex(T item) {
        if(nodes.ContainsKey(item)) {
            return false;
        } else {
            nodes.Add(item, new(item, this));
            return true;
        }
    }

    //only adds the edge if the given nodes allready exist
    public bool AddEdge(T item0, T item1) {
        if(!nodes.TryGetValue(item0, out GraphNode<T>? node0) ||
            !nodes.TryGetValue(item1, out GraphNode<T>? node1)) {
            return false;
        }

        return node0.AddEdgeTo(node1);
    }

    public bool AddEdge(GraphNode<T> node0, GraphNode<T> node1) {
        if(!Equals(node0.Graph) || !Equals(node1.Graph)) {
            throw new ArgumentException("One of the given nodes was not from the relation.");
        }

        return node0.AddEdgeTo(node1);
    }

    public bool ContainsValue(T item) {
        return nodes.ContainsKey(item);
    }

    public bool ContainsEdge(T item0, T item1) {
        return nodes.TryGetValue(item0, out GraphNode<T>? node0) && node0.OutwardEdges.ContainsKey(item1);
    }

    public bool ContainsEdge(GraphNode<T> node0, GraphNode<T> node1) {
        bool e0 = !Equals(node0.Graph);
        bool e1 = !Equals(node1.Graph);

        if(e0 && e1) {
            throw new ArgumentException($"The relation of {node0} and {node1}, did not match the relation it was given to.");
        } else if(e0) {
            throw new ArgumentException($"The relation of {node0}, did not match the relation it was given to.");
        } else if(e1) {
            throw new ArgumentException($"The relation of {node1}, did not match the relation it was given to.");
        }

        return node0.OutwardEdges.ContainsKey(node1.Value);
    }

    public bool TryGetValue(T item, [NotNullWhen(true)] out GraphNode<T>? result) {
        return nodes.TryGetValue(item, out result);
    }

    public void Clear() {
        nodes.Clear();
    }

    public bool RemoveNode(T item) {
        if(nodes.TryGetValue(item, out GraphNode<T>? node)) {
            bool b0 = false;
            foreach((T _, GraphNode<T> n) in node.OutwardEdges) b0 |= n.CutFrom(node);
            bool b1 = nodes.Remove(item);
            return b0 || b1;
        } else {
            return false;
        }
    }

    public bool RemoveNode(GraphNode<T> node) {
        if(Equals(node.Graph)) {
            bool b0 = false;
            foreach((T _, GraphNode<T> n) in node.OutwardEdges) b0 |= n.CutFrom(node);
            bool b1 = nodes.Remove(node.Value);
            return b0 || b1;
        } else {
            return false;
        }
    }

    public bool RemoveEdge(T item0, T item1) {
        if(nodes.TryGetValue(item0, out GraphNode<T>? node0) &&
                nodes.TryGetValue(item1, out GraphNode<T>? node1)) {
            return node0.RemoveEdgeTo(node1);
        } else {
            return false;
        }
    }

    public bool RemoveEdge(GraphNode<T> node0, GraphNode<T> node1) {
        if(Equals(node0.Graph) && Equals(node1.Graph)) {
            return node0.RemoveEdgeTo(node1);
        } else {
            return false;
        }
    }

    public bool IsReflexive() {
        return nodes.All(p => p.Value.OutwardEdges.ContainsKey(p.Key));
    }

    public bool IsSymetric() {
        foreach((T t, GraphNode<T> node) in nodes) {
            foreach((T t2, GraphNode<T> node2) in node.OutwardEdges) {
                if(!node2.OutwardEdges.ContainsKey(t2)) {
                    return false;
                }
            }
        }

        return true;
    }

    public bool IsAntisymetric() {
        foreach((T t, GraphNode<T> node) in nodes) {
            foreach((T t2, GraphNode<T> node2) in node.OutwardEdges) {
                if(!t.Equals(t2) && node2.OutwardEdges.ContainsKey(t2)) {
                    return false;
                }
            }
        }

        return true;
    }

    public bool IsTransitive() {
        throw new NotImplementedException();
    }


    public IEnumerator<GraphNode<T>> GetEnumerator() => nodes.Select(k => k.Value).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => nodes.Select(k => k.Value).GetEnumerator();
}