using HGUtils.Exceptions.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HGUtils.Exceptions.ViewModels
{
    public class ErrorItemViewModel
    {
        [JsonPropertyName("errorCode")]
        public int Code { get; set; }
        [JsonPropertyName("errorMessage")]
        public string Reason { get; set; }
        [JsonPropertyName("developErrorDetail")]
        public ExceptionInfoDetail DevelopErrorDetail { get; set; }
    }
}
