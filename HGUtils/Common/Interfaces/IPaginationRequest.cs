namespace HGUtils.Common.Interfaces
{
    public interface IPaginationRequest
    {
        int? PageNumber { get; set; }
        int? PageSize { get; set; }
    }
}
