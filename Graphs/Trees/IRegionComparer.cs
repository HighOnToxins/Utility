namespace Graphs.Trees;

public interface IRegionComparer<ItemT, RegionT> {
    public int Compare(RegionT region, ItemT item);
}