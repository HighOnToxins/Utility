
using System.Diagnostics.CodeAnalysis;

namespace Graphs.Trees;

//public sealed class RegionTreeNode<ItemT, RegionT> {

//    public ItemT? Value { get; private init; }

//    public RegionT? Region { get; private init; }

//    public RegionTreeNode<ItemT, RegionT>? Parent { get; private init; }

//    public Dictionary<int, RegionTreeNode<ItemT, RegionT>> SubRegions { get; private init; }

//    public bool IsLeaf { get => Value != null; }
//    public bool IsRoot { get => Parent == null; }

//    public RegionTreeNode(ItemT? value) {
//        Value = value;
//        SubRegions = new();
//    }

//    internal void Add(ItemT value) {
//        throw new NotImplementedException();
//    }
//}

public abstract class RegionTreeNode<T, R> {

    public RegionTree<T, R>? Tree { get; set; } 

    public RegionTreeNode<T, R>? Parent { get; private init; }

    internal protected RegionTreeNode() {

    }

    internal protected RegionTreeNode(RegionTree<T, R>? tree, RegionTreeNode<T, R>? parent) {
        Tree = tree;
        Parent = parent;
    }

    public bool TryGetAsRegionNode([NotNullWhen(true)] out RegionNode<T, R>? result) {
        if(this is RegionNode<T, R> regionNode) {
            result = regionNode;
            return true;
        } else {
            result = null;
            return false;
        }
    }

    public bool TryGetAsValueNode([NotNullWhen(true)] out ValueNode<T, R>? result) {
        if(this is ValueNode<T, R> valueNode) {
            result = valueNode;
            return true;
        } else {
            result = null;
            return false;
        }
    }

    internal ValueNode<T, R>? Find(T value) {
        if(TryGetAsValueNode(out ValueNode<T, R>? valueNode)) {
            foreach(T val in valueNode.Values) {
                if(val != null && val.Equals(value)) return valueNode;
            }

            return null;
        }else if(TryGetAsRegionNode(out RegionNode<T, R>? regionNode)) {
            foreach((int _, RegionTreeNode<T, R> node) in regionNode.SubRegions) {
                ValueNode<T, R>? result = node.Find(value);
                if(result != null) return result;
            }
        }

        return null;
    }

    public IEnumerable<T> GetValues() {
        throw new NotImplementedException();
    }
}

public sealed class RegionNode<T, R> : RegionTreeNode<T, R>{

    public R Region { get; private init; }

    private readonly Dictionary<int, RegionTreeNode<T, R>> subRegions;

    public IDictionary<int, RegionTreeNode<T, R>> SubRegions { get => subRegions; }

    public RegionNode(R region) {
        Region = region;
        subRegions = new();
    }

    public RegionNode(R region, RegionTree<T, R>? tree, RegionTreeNode<T, R>? parent) : base(tree, parent) {
        Region = region;
        subRegions = new();
    }

    internal void Add(T? value) {
        throw new NotImplementedException();
    }

    internal void Remove(RegionTreeNode<T, R> regionNode) {
        throw new NotImplementedException();
    }
}

public sealed class ValueNode<T, R>: RegionTreeNode<T, R> {

    private readonly Stack<T> values;

    public IReadOnlyCollection<T> Values { get => values; }

    public ValueNode(params T[] values) {
        this.values = new();
        foreach(T value in values) {
            this.values.Push(value);
        }
    }

    internal void Remove() {
        values.Pop();
    }
}