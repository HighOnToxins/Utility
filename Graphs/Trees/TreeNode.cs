
namespace Graphs.Trees;

public sealed class TreeNode<T> {

    private T value;
    
    public T Value { get => value; set => this.value = value; }

    public ref T ValueRef { get => ref value; }

    public BinaryTreeNode<T>? Parent { get; private set; }

    private readonly List<BinaryTreeNode<T>> children;

    public IReadOnlyList<BinaryTreeNode<T>> Children { get => children; }

    public BinaryTree<T>? Tree { get; internal set; }

    public bool IsLeaf { get => children.Count == 0; }
    public bool IsRoot { get => Parent != null; }

    public TreeNode(T value) {
        this.value = value;
        children = new();
    }

    internal TreeNode(T value, BinaryTree<T>? tree, BinaryTreeNode<T>? parent) : this(value){
        Tree = tree;
        Parent = parent;
    }

}