using System;
using System.Collections.Generic;
using System.Text;

namespace App.Global.Commons.GenericApis
{
    public class GenericActionResult
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
