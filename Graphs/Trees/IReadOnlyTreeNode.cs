namespace Graphs.Trees;

public interface IReadOnlyTreeNode<T> {

    public T Value { get; }

    public IReadOnlyTreeNode<T>? Parent { get; }

    public IReadOnlyTree<T>? Tree { get; }

    public bool IsLeaf { get; }
    public bool IsRoot { get; }

    public IEnumerable<IReadOnlyTreeNode<T>> GetChildren();

}