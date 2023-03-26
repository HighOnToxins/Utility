﻿namespace Graphs.Trees;

public sealed class BinaryTreeNode<T>: IReadOnlyTreeNode<T> {

    public T Value { get; private set; }

    public BinaryTreeNode<T>? Parent { get; private set; }
    public BinaryTreeNode<T>? LeftChild { get; internal set; }
    public BinaryTreeNode<T>? RightChild { get; internal set; }

    public BinaryTree<T>? Tree { get; internal set; }

    public bool IsLeaf { get => LeftChild != null && RightChild != null; }
    public bool IsRoot { get => Parent != null; }

    public bool IsLeftChild { get; private set; }
    public bool IsRightChild { get; private set; }

    IReadOnlyTreeNode<T>? IReadOnlyTreeNode<T>.Parent => Parent;

    IReadOnlyTree<T>? IReadOnlyTreeNode<T>.Tree => Tree;

    public IEnumerable<IReadOnlyTreeNode<T>> GetChildren() {
        if(LeftChild != null) yield return LeftChild;
        if(RightChild != null) yield return RightChild;
    }

    public BinaryTreeNode(T value) {
        Value = value;
    }

    internal BinaryTreeNode(T value, BinaryTreeNode<T>? parent, BinaryTree<T>? tree, bool isLeftChild, bool isRightChild) {
        Value = value;
        Parent = parent;
        Tree = tree;
        IsLeftChild = isLeftChild;
        IsRightChild = isRightChild;
    }

    internal void Add(T value) {
        if(Tree == null) throw new InvalidOperationException("TreeNode did not contain refference to a tree.");
        if(Tree.Comparer.Compare(Value, value) < 0) {
            if(LeftChild == null) {
                LeftChild = new BinaryTreeNode<T>(value, this, Tree, true, false);
            } else {
                LeftChild.Add(value);
            }
        } else {
            if(RightChild == null) {
                RightChild = new BinaryTreeNode<T>(value, this, Tree, false, true);
            } else {
                RightChild.Add(value);
            }
        }
    }

    internal void Add(BinaryTreeNode<T> node) {
        if(Tree == null) throw new InvalidOperationException("TreeNode did not contain refference to a tree.");
        if(Tree.Comparer.Compare(Value, node.Value) < 0) {
            if(LeftChild == null) {
                LeftChild = node;
                node.Parent = this;
            } else {
                LeftChild.Add(node);
            }
        } else {
            if(RightChild == null) {
                RightChild = node;
                node.Parent = this;
            } else {
                RightChild.Add(node);
            }
        }
    }

    internal BinaryTreeNode<T>? Find(T value) {
        if(Tree == null) throw new InvalidOperationException("TreeNode did not contain refference to a tree.");
        int result = Tree.Comparer.Compare(Value, value);

        if(result == 0) {
            return this;
        } else if(result < 0) {
            return LeftChild;
        } else {
            return RightChild;
        }
    }

    internal BinaryTreeNode<T>? FindLast(T value) {
        if(Tree == null) throw new InvalidOperationException("TreeNode did not contain refference to a tree.");

        int result = Tree.Comparer.Compare(Value, value);

        if(result == 0 && RightChild != null && Tree.Comparer.Compare(RightChild.Value, value) == 0) {
            return this;
        } else if(result < 0) {
            return LeftChild;
        } else {
            return RightChild;
        }
    }

    internal bool Contains(T value) {
        if(Tree == null) {
            throw new InvalidOperationException("TreeNode did not contain refference to a tree.");
        }

        int result = Tree.Comparer.Compare(Value, value);

        if(result == 0) {
            return true;
        } else if(result < 0) {
            return LeftChild != null && LeftChild.Contains(value);
        } else {
            return RightChild != null && RightChild.Contains(value);
        }
    }

    internal void PreorderVisit(Action<T> action) {
        action.Invoke(Value);
        LeftChild?.PreorderVisit(action);
        RightChild?.PreorderVisit(action);
    }

    internal void InorderVisit(Action<T> action) {
        LeftChild?.PreorderVisit(action);
        action.Invoke(Value);
        RightChild?.PreorderVisit(action);
    }

    internal void PostorderVisit(Action<T> action) {
        LeftChild?.PreorderVisit(action);
        RightChild?.PreorderVisit(action);
        action.Invoke(Value);
    }

    internal T Max() {
        if(RightChild == null) {
            return Value;
        } else {
            return RightChild.Max();
        }
    }

    internal T Min() {
        if(LeftChild == null) {
            return Value;
        } else {
            return LeftChild.Max();
        }
    }

    internal BinaryTreeNode<T> MaxNode() {
        if(RightChild == null) {
            return this;
        } else {
            return RightChild.MaxNode();
        }
    }

    internal BinaryTreeNode<T> MinNode() {
        if(LeftChild == null) {
            return this;
        } else {
            return LeftChild.MinNode();
        }
    }

    internal IEnumerable<T> PreorderGetValues() {
        IEnumerable<T> values = new T[] { Value };
        if(LeftChild != null) values = values.Concat(LeftChild.InorderGetValues());
        if(RightChild != null) values = values.Concat(RightChild.InorderGetValues());
        return values;
    }

    internal IEnumerable<T> InorderGetValues() {
        IEnumerable<T> values = Enumerable.Empty<T>();
        if(LeftChild != null) values = values.Concat(LeftChild.InorderGetValues());
        values = values.Concat(new T[] { Value });
        if(RightChild != null) values = values.Concat(RightChild.InorderGetValues());
        return values;
    }

    internal IEnumerable<T> PostorderGetValues() {
        IEnumerable<T> values = Enumerable.Empty<T>();
        if(LeftChild != null) values = values.Concat(LeftChild.InorderGetValues());
        if(RightChild != null) values = values.Concat(RightChild.InorderGetValues());
        values = values.Concat(new T[] { Value });
        return values;
    }

    public bool IsDecendantOf(T value) {
        if(Tree == null) {
            throw new InvalidOperationException("TreeNode did not contain refference to a tree.");
        } else {
            return Parent != null && (Tree.Comparer.Compare(Parent.Value, value) == 0 || Parent.IsDecendantOf(value));
        }
    }

    public bool IsAncestorOf(T value) {
        if(Tree == null) {
            throw new InvalidOperationException("TreeNode did not contain refference to a tree.");
        } else if(IsLeaf) {
            return false;
        } else if(Tree.Comparer.Compare(Value, value) < 0) {
            return LeftChild != null && LeftChild.IsAncestorOf(value);
        } else {
            return RightChild != null && RightChild.IsAncestorOf(value);
        }
    }

    public bool IsDecendantOf(BinaryTreeNode<T> node) {
        if(Tree == null || node.Tree == null) {
            throw new InvalidOperationException("TreeNode did not contain refference to a tree.");
        } else if(!node.Tree.Equals(node.Tree)) {
            throw new InvalidOperationException("The given Tree node is not in the tree of the current tree node.");
        } else {
            return IsDecendantOf(node.Value);
        }
    }

    public bool IsAncestorOf(BinaryTreeNode<T> node) {
        if(Tree == null || node.Tree == null) {
            throw new InvalidOperationException("TreeNode did not contain refference to a tree.");
        } else if(!node.Tree.Equals(node.Tree)) {
            throw new InvalidOperationException("The given Tree node is not in the tree of the current tree node.");
        } else {
            return IsAncestorOf(node.Value);
        }
    }

    public bool IsDecendantOf(IReadOnlyTreeNode<T> node) => IsDecendantOf(node);

    public bool IsAncestorOf(IReadOnlyTreeNode<T> node) => IsAncestorOf(node);

}