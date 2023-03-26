namespace Graphs.Trees;

public interface IReadOnlyTree<T>: IReadOnlyCollection<T>, IEnumerable<T> {

    public int GetHeight();

    public int GetWidth();

}