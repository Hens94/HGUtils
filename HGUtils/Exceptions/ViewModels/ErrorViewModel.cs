using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HGUtils.Exceptions.ViewModels
{
    public class ErrorViewModel
    {
        [JsonPropertyName("errors")]
        [JsonProperty("errors")]
        public IEnumerable<ErrorItemViewModel> Errors { get; set; }
    }
}
