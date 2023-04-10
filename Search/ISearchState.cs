namespace Search;

public interface ISearchState
{
    public IEnumerable<ISearchState> GetSearchOptions();
}
