using HGUtils.Common.Enums;
using HGUtils.Common.Interfaces;

namespace HGUtils.Common.Models
{
    public class Result : IResult
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string DetailMessage { get; set; }

        public Result() { }

        public Result(string message, ResultType resultType = ResultType.ApiError, string detailMessage = null)
        {
            Code = (int)resultType;
            Message = message;
            DetailMessage = detailMessage ?? message;
        }
    }

    public class Result<T> : Result, IResult<T> where T : class
    {
        public T Results { get; set; }

        public Result() { }

        public Result(T data, ResultType resultType = ResultType.Success, string message = "Se ha obtenido la información correctamente")
        {
            Code = (int)resultType;
            Message = DetailMessage = message;
            Results = data;
        }

        public Result(string message, ResultType resultType = ResultType.ApiError, string detailMessage = null)
        {
            Code = (int)resultType;
            Message = message;
            DetailMessage = detailMessage ?? message;
            Results = null;
        }
    }

    public class ResultWithPagination<TResult> : Result<TResult> where TResult : class
    {
        public IPaginationResponse Pagination { get; set; }

        public ResultWithPagination(
            TResult data,
            IPaginationResponse pagination) : base(data)
        {
            Pagination = pagination;
        }
    }
}
