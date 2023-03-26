﻿
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Graphs.Relations;

public class Relation<T>: IEnumerable<RelationNode<T>> where T : notnull {

    private readonly Dictionary<T, RelationNode<T>> nodes;

    public int Count => nodes.Count;

    public int RelationCount { get; private set; }

    public Relation() {
        nodes = new();
        RelationCount = 0;
    }

    public bool AddEdge(T item0, T item1) {
        if(!nodes.TryGetValue(item0, out RelationNode<T>? node0) ||
            !nodes.TryGetValue(item1, out RelationNode<T>? node1)) {
            return false;
        }

        node0.AddEdgeTo(node1);
        return true;
    }

    public void AddEdge(RelationNode<T> node0, RelationNode<T> node1) {
        if(!Equals(node0.Relation) || !Equals(node1.Relation)) {
            throw new ArgumentException("One of the given nodes was not from the relation.");
        }

        node0.AddEdgeTo(node1);
    }

    public bool ContainsVertex(T item) {
        return nodes.ContainsKey(item);
    }

    public bool ContainsEdge(T item0, T item1) {
        return nodes.TryGetValue(item0, out RelationNode<T>? node0) && node0.RelationsTo.ContainsKey(item1);
    }

    public bool TryGetVertex(T item, [NotNullWhen(true)] out RelationNode<T>? result) {
        return nodes.TryGetValue(item, out result);
    }

    public void Clear() {
        nodes.Clear();
    }

    public bool RemoveVertex(T item) {
        if(nodes.TryGetValue(item, out RelationNode<T>? node)) {
            foreach((T _, RelationNode<T> n) in node.RelationsTo) n.CutFrom(node);
            nodes.Remove(item);
            return true;
        } else {
            return false;
        }
    }

    public bool RemoveVertex(RelationNode<T> node) {
        if(Equals(node.Relation)) {
            foreach((T _, RelationNode<T> n) in node.RelationsTo) n.CutFrom(node);
            nodes.Remove(node.Value);
            return true;
        } else {
            return false;
        }
    }

    public bool RemoveEdge(T item0, T item1) {
        if(nodes.TryGetValue(item0, out RelationNode<T>? node0) &&
                nodes.TryGetValue(item1, out RelationNode<T>? node1)) {
            node0.RemoveEdgeTo(node1);
            if(node0.RelationsTo.Count == 0 && node0.RelationsFrom.Count == 0) {
                nodes.Remove(node0.Value);
            }
            if(node1.RelationsTo.Count == 0 && node1.RelationsFrom.Count == 0) {
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
            if(node0.RelationsTo.Count == 0 && node0.RelationsFrom.Count == 0) {
                nodes.Remove(node0.Value);
            }
            if(node1.RelationsTo.Count == 0 && node1.RelationsFrom.Count == 0) {
                nodes.Remove(node1.Value);
            }
            return true;
        } else {
            return false;
        }
    }

    public void IntersectWith(Relation<T> relation) {
        throw new NotImplementedException();
    }

    public bool IsProperSubsetOf(Relation<T> relation) {
        throw new NotImplementedException();
    }

    public bool IsProperSupersetOf(Relation<T> relation) {
        throw new NotImplementedException();
    }

    public bool IsSubsetOf(Relation<T> relation) {
        throw new NotImplementedException();
    }

    public bool IsSupersetOf(Relation<T> relation) {
        throw new NotImplementedException();
    }

    public bool Overlaps(Relation<T> relation) {
        throw new NotImplementedException();
    }

    public void SetEquals(Relation<T> relation) {
        throw new NotImplementedException();
    }

    public bool IsReflexive() {
        return nodes.All(p => p.Value.RelationsTo.ContainsKey(p.Key));
    }

    public bool IsSymetric() {
        foreach((T t, RelationNode<T> node) in nodes) {
            foreach((T t2, RelationNode<T> node2) in node.RelationsTo) {
                if(!node2.RelationsTo.ContainsKey(t2)) {
                    return false;
                }
            }
        }

        return true;
    }

    public bool IsAntisymetric() {
        foreach((T t, RelationNode<T> node) in nodes) {
            foreach((T t2, RelationNode<T> node2) in node.RelationsTo) {
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


    public IEnumerator<RelationNode<T>> GetEnumerator() => nodes.Select(k => k.Value).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => nodes.Select(k => k.Value).GetEnumerator();
}