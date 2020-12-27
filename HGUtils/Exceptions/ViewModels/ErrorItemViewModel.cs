using HGUtils.Exceptions.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HGUtils.Exceptions.ViewModels
{
    public class ErrorItemViewModel
    {
        [JsonPropertyName("errorCode")]
        [JsonProperty("errorCode")]
        public int Code { get; set; }
        [JsonPropertyName("errorMessage")]
        [JsonProperty("errorMessage")]
        public string Reason { get; set; }
        [JsonPropertyName("developErrorDetail")]
        [JsonProperty("developErrorDetail")]
        public ExceptionInfoDetail DevelopErrorDetail { get; set; }
    }
}
