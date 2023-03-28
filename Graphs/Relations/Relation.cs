
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Graphs.Relations;

public class Relation<T>: IEnumerable<RelationNode<T>> where T : notnull {

    private readonly Dictionary<T, RelationNode<T>> nodes;

    public int EdgeCount { get; private set; }

    public Relation() {
        nodes = new();
        EdgeCount = 0;
    }

    public bool AddEdge(T item0, T item1) {
        RelationNode<T>? node0;
        if(nodes.TryGetValue(item0, out RelationNode<T>? result0)) {
            node0 = result0;
        } else {
            node0 = new RelationNode<T>(item0, this);
        }

        RelationNode<T>? node1;
        if(nodes.TryGetValue(item1, out RelationNode<T>? result1)) {
            node1 = result1;
        } else {
            node1 = new RelationNode<T>(item1, this);
        }

        return node0.AddEdgeTo(node1);
    }

    public bool AddEdge(RelationNode<T> node0, RelationNode<T> node1) {
        bool e0 = !Equals(node0.Relation);
        bool e1 = !Equals(node1.Relation);

        if(e0 && e1) {
            throw new ArgumentException($"The relation of {node0} and {node1}, did not match the relation it was given to.");
        } else if(e0) {
            throw new ArgumentException($"The relation of {node0}, did not match the relation it was given to.");
        } else if(e1) {
            throw new ArgumentException($"The relation of {node1}, did not match the relation it was given to.");
        }

        return node0.AddEdgeTo(node1);
    }

    public bool ContainsEdge(T item0, T item1) {
        return nodes.TryGetValue(item0, out RelationNode<T>? node0) && 
            node0.OutwardEdges.ContainsKey(item1);
    }

    public bool ContainsEdge(RelationNode<T> node0, RelationNode<T> node1) {
        bool e0 = !Equals(node0.Relation);
        bool e1 = !Equals(node1.Relation);

        if(e0 && e1) {
            throw new ArgumentException($"The relation of {node0} and {node1}, did not match the relation it was given to.");
        } else if(e0) {
            throw new ArgumentException($"The relation of {node0}, did not match the relation it was given to.");
        } else if(e1) {
            throw new ArgumentException($"The relation of {node1}, did not match the relation it was given to.");
        }

        return node0.OutwardEdges.ContainsKey(node1.Value);
    }

    public bool TryGetNode(T item, [NotNullWhen(true)] out RelationNode<T>? result) {
        return nodes.TryGetValue(item, out result);
    }

    public void Clear() {
        nodes.Clear();
    }

    public bool RemoveEdge(T item0, T item1) {
        if(nodes.TryGetValue(item0, out RelationNode<T>? node0) &&
                nodes.TryGetValue(item1, out RelationNode<T>? node1)) {
            
            node0.RemoveEdgeTo(node1);
            if(node0.OutwardEdges.Count == 0 && node0.InwardEdges.Count == 0) {
                nodes.Remove(node0.Value);
            }
            if(node1.OutwardEdges.Count == 0 && node1.InwardEdges.Count == 0) {
                nodes.Remove(node1.Value);
            }
            return true;
        } else {
            return false;
        }
    }

    public bool RemoveEdge(RelationNode<T> node0, RelationNode<T> node1) {
        if(Equals(node0.Relation) && Equals(node1.Relation)) {
            node0.RemoveEdgeTo(node1);
            if(node0.OutwardEdges.Count == 0 && node0.InwardEdges.Count == 0) {
                nodes.Remove(node0.Value);
            }
            if(node1.OutwardEdges.Count == 0 && node1.InwardEdges.Count == 0) {
                nodes.Remove(node1.Value);
            }
            return true;
        } else {
            return false;
        }
    }

    public bool IsReflexive() {
        return nodes.All(p => p.Value.OutwardEdges.ContainsKey(p.Key));
    }

    public bool IsSymetric() {
        foreach((T t, RelationNode<T> node) in nodes) {
            foreach((T t2, RelationNode<T> node2) in node.OutwardEdges) {
                if(!node2.OutwardEdges.ContainsKey(t2)) {
                    return false;
                }
            }
        }

        return true;
    }

    public bool IsAntisymetric() {
        foreach((T t, RelationNode<T> node) in nodes) {
            foreach((T t2, RelationNode<T> node2) in node.OutwardEdges) {
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


    public IEnumerator<RelationNode<T>> GetEnumerator() => nodes.Select(k => k.Value).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => nodes.Select(k => k.Value).GetEnumerator();
}