﻿using HGUtils.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HGUtils.Exceptions.Models
{
    public class ExceptionInfoDetail
    {
        public string Reason { get; set; } = "Undefined";
        public string Detail { get; set; } = "Undefined";
        public DateTime Time => DateTime.Now;
        public Layer Layer { get; set; } = Layer.Undefined;
        public string Service { get; set; } = "Undefined";
        public string Operation { get; set; } = "Undefined";
        public string ExceptionName { get; set; } = "Undefined";
        public ExceptionInfoDetail InnerError { get; set; }
    }
}