namespace HGUtils.Common.Interfaces
{
    public interface IPaginationResponse
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
        int TotalPage { get; set; }
        int TotalItems { get; set; }
    }
}
