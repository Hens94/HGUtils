using HGUtils.Common.Interfaces;

namespace HGUtils.Helpers.Common.Pagination
{
    public static class PaginationHelper
    {
        public static bool UsePagination(this IPaginationRequest pagination) =>
            pagination is null ?
            false :
            !(pagination.PageNumber is null) || !(pagination.PageSize is null);
    }
}
