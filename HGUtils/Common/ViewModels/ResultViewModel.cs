using HGUtils.Common.Interfaces;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HGUtils.Common.ViewModels
{
    public class ResultViewModel : IResult
    {
        [JsonPropertyName("code")]
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonPropertyName("detailMessage")]
        [JsonProperty("detailMessage")]
        public string DetailMessage { get; set; }
    }

    public class ResultViewModel<T> : ResultViewModel, IResult<T> where T : class
    {
        [JsonPropertyName("results")]
        [JsonProperty("results")]
        public T Results { get; set; }
    }

    public class ResultWithPaginationViewModel<TResult> : ResultViewModel<TResult> where TResult : class
    {
        [JsonPropertyName("pagination")]
        [JsonProperty("pagination")]
        public IPaginationResponse Pagination { get; set; }
    }
}
