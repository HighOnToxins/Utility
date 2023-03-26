namespace Graphs.Trees;

public interface IReadOnlyTree<T>: IEnumerable<T> {

    public IReadOnlyTree<T> GetSubTree(T value);

    public IReadOnlyTree<T> GetSubTree(IReadOnlyTreeNode<T> node);

    public IReadOnlyTreeNode<T>? Find(T value);

    public IReadOnlyTreeNode<T>? FindLast(T value);

    public bool Contains(T value);

    public void Visit(Action<T> action, int selfIndex = 0);

    public IEnumerable<T> GetValues(int selfIndex = 0);

}