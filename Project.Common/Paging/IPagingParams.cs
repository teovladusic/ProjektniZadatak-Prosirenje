namespace Common
{
    public interface IPagingParams
    {
        int CurrentPage { get; set; }
        int PageSize { get; set; }
    }
}