﻿using System.Collections;

namespace Graphs.Trees;

//TODO: RegionQuadTree
//TODO: RegionOctTree

public class BinaryTree<T>: IReadOnlyTree<T>, IEnumerable<T> {

    internal IComparer<T> Comparer { get; private init; }

    public BinaryTreeNode<T>? Root { get; private set; }

    public BinaryTree(IComparer<T> comparer) {
        Comparer = comparer;
    }

    public int GetHeight() {
        throw new NotImplementedException();
    }

    public void Add(T value) {
        if(Root == null) {
            Root = new BinaryTreeNode<T>(value, null, this, false, false);
        } else {
            Root.Add(value);
        }
    }

    public void Add(BinaryTreeNode<T> node) {
        if(node.Tree != null) {
            throw new ArgumentException("The Tree node already belongs to a Tree.");
        } else if(Root == null) {
            Root = node;
        } else {
            Root.Add(node);
        }
    }
    public void AddRoot(T value) {
        BinaryTreeNode<T> node = new(value, null, this, false, false); ;
        if(Root != null) {
            BinaryTreeNode<T> prevRoot = Root;
            Root = node;
            Root.Add(prevRoot);
        } else {
            Root = node;
        }
    }

    public void AddRoot(BinaryTreeNode<T> node) {
        if(node.Tree != null) {
            throw new ArgumentException("The Tree node already belongs to a Tree.");
        } else if(Root != null) {
            BinaryTreeNode<T> prevRoot = Root;
            Root = node;
            node.Add(prevRoot);
        } else {
            Root = node;
        }
    }

    public void Clear() {
        Root = null;
    }

    public void RemoveRoot() {
        if(Root == null) {
            throw new InvalidOperationException();
        }

        BinaryTreeNode<T> prevRoot = Root;
        Root = null;
        if(prevRoot.LeftChild != null) Add(prevRoot.LeftChild);
        if(prevRoot.RightChild != null) Add(prevRoot.RightChild);
    }

    public bool Remove(T value) {
        BinaryTreeNode<T>? node = Find(value);

        if(node == null) {
            return false;
        } else if(node.Parent == null) {
            Root = null;
            return true;
        } else if(node.IsLeftChild) {
            node.Parent.LeftChild = null;
        } else {
            node.Parent.RightChild = null;
        }

        if(node.LeftChild != null) node.Parent.Add(node.LeftChild);
        if(node.RightChild != null) node.Parent.Add(node.RightChild);

        return true;
    }

    public void Remove(BinaryTreeNode<T> node) {
        if(node == null) {
            throw new ArgumentNullException();
        } else if(!Equals(node.Tree)) {
            throw new InvalidOperationException();
        } else if(node.Parent == null) {
            Root = null;
            return;
        } else if(node.IsLeftChild) {
            node.Parent.LeftChild = null;
        } else {
            node.Parent.RightChild = null;
        }

        if(node.LeftChild != null) node.Parent.Add(node.LeftChild);
        if(node.RightChild != null) node.Parent.Add(node.RightChild);
    }

    public bool RemoveSubTree(T value) {
        BinaryTreeNode<T>? node = Find(value);

        if(node == null || Root == null) {
            return false;
        } else if(node.Parent == null) {
            Root = null;
        } else if(node.IsLeftChild) {
            node.Parent.LeftChild = null;
        } else {
            node.Parent.RightChild = null;
        }

        return true;
    }

    public void RemoveSubTree(BinaryTreeNode<T> node) {
        if(node == null) {
            throw new ArgumentNullException();
        } else if(!Equals(node.Tree)) {
            throw new InvalidOperationException("The tree of the given node did not match the tree it was given to.");
        } else if(node.Parent == null) {
            Root = null;
        } else if(node.IsLeftChild) {
            node.Parent.LeftChild = null;
        } else {
            node.Parent.RightChild = null;
        }
    }

    public BinaryTree<T> GetSubTree(T value) {
        BinaryTreeNode<T>? node = Find(value);

        if(node == null) {
            throw new ArgumentException("The given value did not exist within the tree!");
        }

        BinaryTree<T> subTree = new(Comparer);
        foreach(T t in node.PreorderGetValues()) {
            subTree.Add(t);
        }
        return subTree;
    }

    public BinaryTree<T> GetSubTree(BinaryTreeNode<T> node) {
        if(node == null) {
            throw new ArgumentException("The given value did not exist within the tree!");
        } else if(!Equals(node.Tree)) {
            throw new ArgumentException("The tree of the given node did not match the tree it was given to.");
        }

        BinaryTree<T> subTree = new(Comparer);
        foreach(T t in node.PreorderGetValues()) {
            subTree.Add(t);
        }
        return subTree;
    }

    public BinaryTreeNode<T>? Find(T value) {
        return Root?.Find(value);
    }

    public BinaryTreeNode<T>? FindLast(T value) {
        return Root?.FindLast(value);
    }

    public T Max() {
        if(Root == null) {
            throw new ArgumentOutOfRangeException();
        } else {
            return Root.Max();
        }
    }

    public T Min() {
        if(Root == null) {
            throw new ArgumentOutOfRangeException();
        } else {
            return Root.Min();
        }
    }

    public BinaryTreeNode<T> MaxNode() {
        if(Root == null) {
            throw new ArgumentOutOfRangeException();
        } else {
            return Root.MaxNode();
        }
    }

    public BinaryTreeNode<T> MinNode() {
        if(Root == null) {
            throw new ArgumentOutOfRangeException();
        } else {
            return Root.MinNode();
        }
    }

    public bool Contains(T value) {
        return Root != null && Root.Contains(value);
    }

    public void PreorderVisit(Action<T> action) {
        Root?.PreorderVisit(action);
    }

    public void InorderVisit(Action<T> action) {
        Root?.InorderVisit(action);
    }

    public void PostorderVisit(Action<T> action) {
        Root?.PostorderVisit(action);
    }

    public IEnumerable<T> PreorderGetValues() {
        if(Root != null) {
            return Root.PreorderGetValues();
        } else {
            return Enumerable.Empty<T>();
        }
    }

    public IEnumerable<T> InorderGetValues() {
        if(Root != null) {
            return Root.InorderGetValues();
        } else {
            return Enumerable.Empty<T>();
        }
    }

    public IEnumerable<T> PostorderGetValues() {
        if(Root != null) {
            return Root.PostorderGetValues();
        } else {
            return Enumerable.Empty<T>();
        }
    }
    public IEnumerator<T> GetEnumerator() {
        if(Root != null) {
            return Root.InorderGetValues().GetEnumerator();
        } else {
            return Enumerable.Empty<T>().GetEnumerator();
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        if(Root != null) {
            return Root.InorderGetValues().GetEnumerator();
        } else {
            return Enumerable.Empty<T>().GetEnumerator();
        }
    }

    IReadOnlyTree<T> IReadOnlyTree<T>.GetSubTree(T value) => GetSubTree(value);

    public IReadOnlyTree<T> GetSubTree(IReadOnlyTreeNode<T> node) => GetSubTree(node);

    IReadOnlyTreeNode<T>? IReadOnlyTree<T>.Find(T value) => Find(value);

    IReadOnlyTreeNode<T>? IReadOnlyTree<T>.FindLast(T value) => FindLast(value);

    public void Visit(Action<T> action, int selfIndex = 0) {
        if(selfIndex < 0) {
            throw new ArgumentException();
        } else if(selfIndex == 0) {
            PreorderVisit(action);
        } else if(selfIndex == 1) {
            InorderVisit(action);
        } else {
            PostorderVisit(action);
        }
    }

    public IEnumerable<T> GetValues(int selfIndex = 0) {
        if(selfIndex < 0) {
            throw new ArgumentException();
        } else if(selfIndex == 0) {
            return PreorderGetValues();
        } else if(selfIndex == 1) {
            return InorderGetValues();
        } else {
            return PostorderGetValues();
        }
    }
}
