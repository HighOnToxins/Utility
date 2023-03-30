
using System.Xml.Linq;

namespace Graphs.Trees;

public sealed class TreeNode<T> {

    private T value;
    
    public T Value { get => value; set => this.value = value; }

    public ref T ValueRef { get => ref value; }

    public TreeNode<T>? Parent { get; private set; }

    private readonly List<TreeNode<T>> children;

    public IReadOnlyList<TreeNode<T>> Children { get => children; }

    public Tree<T>? Tree { get; internal set; }

    public bool IsLeaf { get => children.Count == 0; }
    public bool IsRoot { get => Parent != null; }

    public TreeNode(T value) {
        this.value = value;
        children = new();
    }

    internal TreeNode(T value, Tree<T>? tree, TreeNode<T>? parent) : this(value){
        Tree = tree;
        Parent = parent;
    }

    internal int GetHeight() {
        int maxHeight = 0;
        foreach(TreeNode<T> child in Children) {
            maxHeight = Math.Max(maxHeight, child.GetHeight() + 1);
        }
        return maxHeight;
    }

    internal void AddChild(TreeNode<T> node) {
        children.Add(node);
        node.Parent = this;
    }

    internal TreeNode<T> Clone() {
        TreeNode<T> node = new(value);
        foreach(TreeNode<T> child in Children) {
            TreeNode<T> clone = child.Clone();
            clone.Parent = this;
            node.AddChild(clone);
        }
        return node;
    }

    internal void AssignTreeToSubTree(Tree<T> tree) {
        Tree = tree;
        foreach(TreeNode<T> child in Children) {
            child.AssignTreeToSubTree(tree);
        }
    }

    internal TreeNode<T>? Find(T value) {
        if(Value != null && Value.Equals(value)) {
            return this;
        }

        foreach(TreeNode<T> child in Children) {
            TreeNode<T>? node = child.Find(value);
            
            if(node != null) {
                return node;
            }
        }

        return null;
    }

    internal bool Contains(T value) {
        if(Value != null && Value.Equals(value)) {
            return true;
        } else {
            return Children.Any(c => c.Contains(value));
        }
    }

    internal void Remove(TreeNode<T> node) {
        children.Remove(node);
    }

    internal void RemoveSubTree(TreeNode<T> node) {
        children.Remove(node);
        foreach(TreeNode<T> child in node.children) {
            AddChild(child);
            child.Parent = this;
        }
    }
}