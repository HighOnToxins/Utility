namespace Search;

public interface IWeightedSearchState
{

    public IEnumerable<(IWeightedSearchState value, double priorityDifference)> GetSearchOptions();

}
