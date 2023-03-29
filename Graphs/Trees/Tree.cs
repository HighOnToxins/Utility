
namespace Graphs.Trees;

public class Tree<T> {

    public TreeNode<T>? Root { get; private set; }

    public int NodeCount;

    public Tree() {
        NodeCount = 0;
    }

    public int GetHeight() {
        throw new NotImplementedException();
    }

    public int GetWidth() {
        throw new NotImplementedException();
    }

    public void AddRoot(T item) {
        throw new NotImplementedException();
    }

    public void AddRoot(TreeNode<T> node) {
        throw new NotImplementedException();
    }

    public void AddChild(T? parentItem, T childItem) {
        throw new NotImplementedException();
    }

    public void AddChild(TreeNode<T>? parentNode, T childItem) {
        throw new NotImplementedException();
    }

    public void AddChild(TreeNode<T>? parentNode, TreeNode<T> childNode) {
        throw new NotImplementedException();
    }

    public void AddTree(T parentItem, Tree<T> tree) {
        throw new NotImplementedException();
    }

    public void AddTree(TreeNode<T> parentNode, Tree<T> tree) {
        throw new NotImplementedException();
    }

    public Tree<T> GetSubTree(T value) {
        throw new NotImplementedException();
    }

    public Tree<T> GetSubTree(TreeNode<T> node) {
        throw new NotImplementedException();
    }

    public TreeNode<T>? Find(T value) {
        throw new NotImplementedException();
    }

    public TreeNode<T>? FindLast(T value) {
        throw new NotImplementedException();
    }

    public bool Contains(T value) {
        throw new NotImplementedException();
    }

    public void RemoveNode(T item) {
        throw new NotImplementedException();
    }

    public void RemoveNode(TreeNode<T> node) {
        throw new NotImplementedException();
    }

    public void RemoveSubTree(T item) {
        throw new NotImplementedException();
    }

    public void RemoveSubTree(TreeNode<T> item) {
        throw new NotImplementedException();
    }
    
}