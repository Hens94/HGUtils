using HGUtils.Exceptions.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HGUtils.Exceptions.ViewModels
{
    public class ErrorViewModel
    {
        [JsonPropertyName("errors")]
        public IEnumerable<ExceptionInfo> Errors { get; set; }
    }
}
