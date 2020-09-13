using System;
using System.Collections.Generic;
using System.Text;

namespace ET.ODataExamples.Storage.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class Summary : Attribute
    {
        public Summary(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}
