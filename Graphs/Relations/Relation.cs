
using System.Collections;

namespace Graphs.Relations;

public class Relation<T>: IEnumerable<RelationNode<T>> where T : notnull {

    private readonly Dictionary<T, RelationNode<T>> nodes;

    public int Count => nodes.Count;

    public int RelationCount { get; private set; }

    public Relation() {
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

    public bool AddVertex(RelationNode<T> node) {
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
        throw new NotImplementedException();
    }

    public bool AddEdge(RelationNode<T> node0, RelationNode<T> node1) {
        throw new NotImplementedException();
    }

    public bool ContainsVertex(T item) {
        throw new NotImplementedException();
    }

    public bool ContainsEdge(T item0, T item1) {
        throw new NotImplementedException();
    }

    public bool TryGetValue(T item, out RelationNode<T> result) {
        throw new NotImplementedException();
    }

    public void Clear() {
        throw new NotImplementedException();
    }

    public bool RemoveVertex(T item) {
        throw new NotImplementedException();
    }

    public bool RemoveVertex(RelationNode<T> node) {
        throw new NotImplementedException();
    }

    public bool RemoveEdge(T item0, T item1) {
        throw new NotImplementedException();
    }

    public bool RemoveEdge(RelationNode<T> node0, RelationNode<T> node1) {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public bool IsSymetric() {
        throw new NotImplementedException();
    }

    public bool IsAntisymetric() {
        throw new NotImplementedException();
    }

    public bool IsTransitive() {
        throw new NotImplementedException();
    }


    public IEnumerator<RelationNode<T>> GetEnumerator() => nodes.Select(k => k.Value).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => nodes.Select(k => k.Value).GetEnumerator();
}