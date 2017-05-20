using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Geolaxia.Models
{
    public class ApiResponse
    {
        public object Data { get; set; }
        public Status Status { get; set; }
        public int Code { get; set; }
    }
}