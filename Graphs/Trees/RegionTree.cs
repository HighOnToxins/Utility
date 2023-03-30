
using System.Collections;

namespace Graphs.Trees;

public class RegionTree<T, R>: IEnumerable<T> {

    private readonly R defaultRegion;

    private readonly IRegionComparer<T, R> comparer;

    public RegionTreeNode<T, R>? Root { get; private set; }

    public RegionTree(IRegionComparer<T, R> comparer, R defaultRegion) {
        this.comparer = comparer;
        this.defaultRegion = defaultRegion;
    }

    public void Add(T value) {
        if(Root == null) {
            Root = new ValueNode<T, R>(value);
        } else if(Root.TryGetAsValueNode(out ValueNode<T, R>? valueNode)){
            RegionNode<T, R> regionRoot = new(defaultRegion, this, null);
            foreach(T val in valueNode.Values) {
                regionRoot.Add(val);
            }
            regionRoot.Add(value);
            Root = regionRoot;
        } else if(Root.TryGetAsRegionNode(out RegionNode<T, R>? regionNode)) {
            regionNode.Add(value);
        }
    }

    public ValueNode<T, R>? Find(T value) {
        if(Root == null) {
            return null;
        } else {
            return Root.Find(value);
        }
    }

    public void Remove(ValueNode<T, R> node) {
        if(node.Values.Count > 1) {
            node.Remove();
        }else if(node.Parent != null && node.Parent.TryGetAsRegionNode(out RegionNode<T, R>? parentRegion)) {
            parentRegion.Remove(node);
        } else {
            Root = null;
        }
    }

    public void RemoveSubTree(RegionTreeNode<T, R> node) {
        if(node.Parent != null && node.Parent.TryGetAsRegionNode(out RegionNode<T, R>? parentRegion)) {
            parentRegion.Remove(node);
        } else {
            Root = null;
        }
    }

    public void Clear() {
        Root = null;
    }

    public IEnumerator<T> GetEnumerator() {
        if(Root == null) {
            return Enumerable.Empty<T>().GetEnumerator();
        } else {
            return Root.GetValues().GetEnumerator();
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        if(Root == null) {
            return Enumerable.Empty<T>().GetEnumerator();
        } else {
            return Root.GetValues().GetEnumerator();
        }
    }
}