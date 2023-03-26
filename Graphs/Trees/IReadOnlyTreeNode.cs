namespace Graphs.Trees;

public interface IReadOnlyTreeNode<T> {

    public T Value { get; }

    public IReadOnlyTreeNode<T>? Parent { get; }

    public IReadOnlyTree<T>? Tree { get; }

    public bool IsLeaf { get => !GetChildren().Any(); }
    public bool IsRoot { get => Parent != null; }

    public IEnumerable<IReadOnlyTreeNode<T>> GetChildren();

    public bool IsDecendantOf(T value);

    public bool IsAncestorOf(T value);

    public bool IsDecendantOf(IReadOnlyTreeNode<T> value);

    public bool IsAncestorOf(IReadOnlyTreeNode<T> value);

}