using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BATTLESHIP_CORE_API.DTO
{
    public class Response
    {
        public string ResponseCode { get; set; }
        public object Data { get; set; }

        public string Message { get; set; }
    }
}
