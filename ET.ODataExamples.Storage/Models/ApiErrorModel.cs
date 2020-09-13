using System;
using System.Collections.Generic;

namespace ET.ODataExamples.Storage.Models
{
    public class ApiErrorModel
    {
        public List<Errors> Errors { get; set; }

    }

    public class Errors
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public object Data { get; set; }
    }
}
