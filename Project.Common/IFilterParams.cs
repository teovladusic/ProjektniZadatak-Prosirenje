namespace Common
{
    public interface IFilterParams
    {
        PagingParams PagingParams { get; set; }
        SortParams SortParams { get; set; }
    }
}