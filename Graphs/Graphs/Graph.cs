
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace Graphs.Relations;

public class Graph<T>: IEnumerable<GraphNode<T>> where T : notnull {

    private readonly Dictionary<T, GraphNode<T>> nodes;

    public int Count => nodes.Count;

    public int RelationCount { get; private set; }

    public Graph() {
        nodes = new();
        RelationCount = 0;
    }

    public bool AddVertex(T item) {
        if(nodes.ContainsKey(item)) {
            return false;
        } else {
            nodes.Add(item, new(item));
            return true;
        }
    }

    public bool AddVertex(GraphNode<T> node) {
        if(node.Relation != null) {
            throw new ArgumentException("The vertex already belongs to a relation.");
        } else if(nodes.ContainsValue(node)) {
            return false;
        } else {
            nodes.Add(node.Value, new(node.Value));
            return true;
        }
    }

    public bool AddEdge(T item0, T item1) {
        if(!nodes.TryGetValue(item0, out GraphNode<T>? node0) ||
            !nodes.TryGetValue(item1, out GraphNode<T>? node1)) {
            return false;
        }

        node0.AddEdgeTo(node1);
        return true;
    }

    public void AddEdge(GraphNode<T> node0, GraphNode<T> node1) {
        if(!Equals(node0.Relation) || !Equals(node1.Relation)) {
            throw new ArgumentException("One of the given nodes was not from the relation.");
        }

        node0.AddEdgeTo(node1);
    }

    public bool ContainsVertex(T item) {
        return nodes.ContainsKey(item);
    }

    public bool ContainsEdge(T item0, T item1) {
        return nodes.TryGetValue(item0, out GraphNode<T>? node0) && node0.RelationsTo.ContainsKey(item1);
    }

    public bool TryGetValue(T item, [NotNullWhen(true)] out GraphNode<T>? result) {
        return nodes.TryGetValue(item, out result);
    }

    public void Clear() {
        nodes.Clear();
    }

    public bool RemoveVertex(T item) {
        if(nodes.TryGetValue(item, out GraphNode<T>? node)) {
            foreach((T _, GraphNode<T> n) in node.RelationsTo) n.CutFrom(node);
            nodes.Remove(item);
            return true;
        } else {
            return false;
        }
    }

    public bool RemoveVertex(GraphNode<T> node) {
        if(Equals(node.Relation)) {
            foreach((T _, GraphNode<T> n) in node.RelationsTo) n.CutFrom(node);
            nodes.Remove(node.Value);
            return true;
        } else {
            return false;
        }
    }

    public bool RemoveEdge(T item0, T item1) {
        if(nodes.TryGetValue(item0, out GraphNode<T>? node0) &&
                nodes.TryGetValue(item1, out GraphNode<T>? node1)) {
            node0.RemoveEdgeTo(node1);
            return true;
        } else {
            return false;
        }
    }

    public bool RemoveEdge(GraphNode<T> node0, GraphNode<T> node1) {
        if(Equals(node0.Relation) && Equals(node1.Relation)) {
            node0.RemoveEdgeTo(node1);
            return true;
        } else {
            return false;
        }
    }

    public void IntersectWith(Graph<T> relation) {
        throw new NotImplementedException();
    }

    public bool IsProperSubsetOf(Graph<T> relation) {
        throw new NotImplementedException();
    }

    public bool IsProperSupersetOf(Graph<T> relation) {
        throw new NotImplementedException();
    }

    public bool IsSubsetOf(Graph<T> relation) {
        throw new NotImplementedException();
    }

    public bool IsSupersetOf(Graph<T> relation) {
        throw new NotImplementedException();
    }

    public bool Overlaps(Graph<T> relation) {
        throw new NotImplementedException();
    }

    public void SetEquals(Graph<T> relation) {
        throw new NotImplementedException();
    }

    public bool IsReflexive() {
        return nodes.All(p => p.Value.RelationsTo.ContainsKey(p.Key));
    }

    public bool IsSymetric() {
        foreach((T t, GraphNode<T> node) in nodes) {
            foreach((T t2, GraphNode<T> node2) in node.RelationsTo) {
                if(!node2.RelationsTo.ContainsKey(t2)) {
                    return false;
                }
            }
        }

        return true;
    }

    public bool IsAntisymetric() {
        foreach((T t, GraphNode<T> node) in nodes) {
            foreach((T t2, GraphNode<T> node2) in node.RelationsTo) {
                if(!t.Equals(t2) && node2.RelationsTo.ContainsKey(t2)) {
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