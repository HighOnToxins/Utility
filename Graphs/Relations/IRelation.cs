
namespace Graphs.Relations; 

public interface IRelation<T> : IEnumerable<IRelationNode<T>>{

    public int Count { get; }
    public int RelationCount { get; }
    
    public void Add(T item0, T item1);

    public bool Contains(T item);
    public bool Contains(T item0, T item1);

    public bool TryGetValue(T item, out IRelationNode<T> result);

    public void Clear();
    public bool Remove(T item);
    public bool Remove(IRelationNode<T> node);
    public bool Remove(T item0, T item1);

    public void IntersectWith(IRelation<T> relation);
    public bool IsProperSubsetOf(IRelation<T> relation);
    public bool IsProperSupersetOf(IRelation<T> relation);
    public bool IsSubsetOf(IRelation<T> relation);
    public bool IsSupersetOf(IRelation<T> relation);
    public bool Overlaps(IRelation<T> relation);
    public void SetEquals(IRelation<T> relation);

}