using HGUtils.Common.Enums;
using HGUtils.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HGUtils.Common.Models
{
    public class Result : IResult
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string DetailMessage { get; set; }

        public Result() { }

        public Result(string message, ResultType resultType = ResultType.Error, string detailMessage = null)
        {
            Code = (int)resultType;
            Message = message;
            DetailMessage = detailMessage ?? message;
        }
    }

    public class Result<T> : Result, IResult<T> where T : class
    {
        public T Data { get; set; }

        public Result() { }

        public Result(T data, ResultType resultType = ResultType.Success, string message = "Se ha obtenido la información correctamente")
        {
            Code = (int)resultType;
            Message = DetailMessage = message;
            Data = data;
        }

        public Result(string message, ResultType resultType = ResultType.Error, string detailMessage = null)
        {
            Code = (int)resultType;
            Message = message;
            DetailMessage = detailMessage ?? message;
            Data = null;
        }
    }
}
