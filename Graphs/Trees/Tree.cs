
namespace Graphs.Trees;

public class Tree<T> {

    public TreeNode<T>? Root { get; private set; }

    public int NodeCount;

    public Tree() {
        NodeCount = 0;
    }

    public int GetHeight() {
        if(Root == null) {
            return 0;
        } else {
            return Root.GetHeight();
        }
    }

    public void AddRoot(T item) {
        if(Root == null) {
            Root = new TreeNode<T>(item, this);
        } else {
            TreeNode<T> prevRoot = Root;
            Root = new TreeNode<T>(item, this);
            Root.AddChild(prevRoot);
        }
        NodeCount++;
    }

    public void AddRoot(TreeNode<T> node) {
        if(node.Tree != null) {
            throw new ArgumentException("The Tree node already belongs to a Tree.");
        } else if(Root == null) {
            Root = node;
        } else {
            TreeNode<T> prevRoot = Root;
            Root = node;
            Root.AddChild(prevRoot);
        }
        NodeCount++;
    }

    public void AddChild(TreeNode<T>? parentNode, T childItem) {
        if(parentNode == null) {
            AddRoot(childItem);
        } else if(!Equals(parentNode.Tree)) {
            throw new ArgumentException("The tree of the given node did not match the tree it was given to.");
        } else {
            parentNode.AddChild(new TreeNode<T>(childItem, this));
            NodeCount++;
        }
    }

    public void AddChild(TreeNode<T>? parentNode, TreeNode<T> childNode) {
        if(parentNode == null) {
            AddRoot(childNode);
        } else if(!Equals(parentNode.Tree)) {
            throw new ArgumentException("The tree of the given node did not match the tree it was given to.");
        } else if(childNode.Tree != null) {
            throw new ArgumentException("The tree node already belongs to a tree.");
        } else { 
            parentNode.AddChild(childNode);
            NodeCount++;
        }
    }

    public void AddTree(TreeNode<T> parentNode, Tree<T> tree) {
        if(!Equals(parentNode.Tree)) {
            throw new ArgumentException("The tree of the given node did not match the tree it was given to.");
        } else if(tree.Root != null) {
            TreeNode<T> node = tree.Root.Clone();
            node.AssignTreeToSubTree(this);
            parentNode.AddChild(node);
        }
        NodeCount += tree.NodeCount;
    }

    public Tree<T> GetSubTree(TreeNode<T> node) {
        if(!Equals(node.Tree)) {
            throw new ArgumentException("The tree of the given node did not match the tree it was given to.");
        } else {
            Tree<T> tree = new() {
                Root = node.Clone()
            };
            tree.Root.AssignTreeToSubTree(tree);
            return tree;
        }
    }

    public TreeNode<T>? Find(T value) {
        return Root?.Find(value);
    }

    public bool Contains(T value) {
        if(Root == null) {
            return false;
        } else {
            return Root.Contains(value);
        }
    }

    public void RemoveNode(TreeNode<T> node) {
        IReadOnlyList<TreeNode<T>> children = node.Children;

        if(!Equals(node.Tree)) {
            throw new ArgumentException("The tree of the given node did not match the tree it was given to.");
        }else if(node.Parent == null) {
            if(children.Count == 0) {
                Root = null;
                return;
            } else {
                throw new InvalidOperationException("The root of a tree can not be removed unless it has no children.");
            }
        }

        node.Parent.Remove(node);
        foreach(TreeNode<T> child in children) {
            node.Parent.AddChild(child.Clone());
        }
        NodeCount--;
    }

    public void RemoveSubTree(TreeNode<T> node) {
        if(!Equals(node.Tree)) {
            throw new ArgumentException("The tree of the given node did not match the tree it was given to.");
        } else if(node.Parent == null) {
            Root = null;
        } else {
            node.Parent.RemoveSubTree(node);
        }
        NodeCount -= node.CountNodes();
    }

    public void Clear() {
        Root = null;
    }
}