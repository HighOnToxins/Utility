
namespace Graphs.Trees;

public interface ITree<T> : ICollection<T> {

    public int GetHeight();

    public int GetWidth();

    public void Add(BinaryTreeNode<T> node);

    public void AddRoot(T value);

    public void AddRoot(BinaryTreeNode<T> node);

    public void RemoveRoot();

    public void Remove(BinaryTreeNode<T> node);

    public bool RemoveSubTree(T value);

    public void RemoveSubTree(BinaryTreeNode<T> node);

    public IReadOnlyTree<T> GetSubTree(T value);

    public IReadOnlyTree<T> GetSubTree(ITreeNode<T> node);

    public ITreeNode<T>? Find(T value);

    public ITreeNode<T>? FindLast(T value);

    public void Visit(Action<T> action, int selfIndex = 0);

    public IEnumerable<T> GetValues(int selfIndex = 0);

}