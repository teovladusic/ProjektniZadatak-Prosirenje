namespace Common
{
    public interface IVehicleMakeFilterParams
    {
        IPagingParams PagingParams { get; set; }
        string SearchQuery { get; set; }
        ISortParams SortParams { get; set; }
    }
}