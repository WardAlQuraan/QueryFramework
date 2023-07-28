using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.API_RESPONSE
{
    public class ApiRespone
    {
        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Message { get; set; }
        public bool IsError { get; set; }
        public object? Data { get; set; }


    }
}
