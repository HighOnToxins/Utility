
using System.Collections;

namespace Graphs.Relations;

public class Relation<T>: IRelation<T>, IReadOnlyRelation<T> where T : notnull {

    private readonly Dictionary<T, IRelationNode<T>> nodes; 

    public int Count => nodes.Count;

    public int RelationCount {get; private set;}

    public Relation() {
        nodes = new();
        RelationCount = 0;
    }


    public void Add(T item0, T item1) {
        throw new NotImplementedException();
    }

    public void Clear() {
        throw new NotImplementedException();
    }

    public bool Contains(T item) {
        throw new NotImplementedException();
    }

    public bool Contains(T item0, T item1) {
        throw new NotImplementedException();
    }

    public IEnumerator<IRelationNode<T>> GetEnumerator() {
        throw new NotImplementedException();
    }

    public void IntersectWith(IRelation<T> relation) {
        throw new NotImplementedException();
    }

    public bool IsProperSubsetOf(IRelation<T> relation) {
        throw new NotImplementedException();
    }

    public bool IsProperSupersetOf(IRelation<T> relation) {
        throw new NotImplementedException();
    }

    public bool IsSubsetOf(IRelation<T> relation) {
        throw new NotImplementedException();
    }

    public bool IsSupersetOf(IRelation<T> relation) {
        throw new NotImplementedException();
    }

    public bool Overlaps(IRelation<T> relation) {
        throw new NotImplementedException();
    }

    public bool Remove(T item) {
        throw new NotImplementedException();
    }

    public bool Remove(IRelationNode<T> node) {
        throw new NotImplementedException();
    }

    public bool Remove(T item0, T item1) {
        throw new NotImplementedException();
    }

    public void SetEquals(IRelation<T> relation) {
        throw new NotImplementedException();
    }

    public bool TryGetValue(T item, out IRelationNode<T> result) {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        throw new NotImplementedException();
    }
}