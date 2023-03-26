namespace Graphs.Trees;

public interface ITreeNode<T> {

    public T Value { get; }

    public ITreeNode<T>? Parent { get; }

    public IReadOnlyTree<T>? Tree { get; }

    public bool IsLeaf { get; }
    public bool IsRoot { get; }

    public IEnumerable<ITreeNode<T>> GetChildren();

    public bool IsDecendantOf(T value);

    public bool IsAncestorOf(T value);

    public bool IsDecendantOf(ITreeNode<T> value);

    public bool IsAncestorOf(ITreeNode<T> value);

}