using HGUtils.Common.Interfaces;
using System.Text.Json.Serialization;

namespace HGUtils.Common.ViewModels
{
    public class ResultViewModel : IResult
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("detailMessage")]
        public string DetailMessage { get; set; }
    }

    public class ResultViewModel<T> : ResultViewModel, IResult<T> where T : class
    {
        [JsonPropertyName("results")]
        public T Results { get; set; }
    }
}
