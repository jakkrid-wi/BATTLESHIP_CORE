using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BATTLESHIP_CORE_API.DTO
{
    public class LocationDTO
    {
        public int SHIP_ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string RESULT { get; set; }
    }
}
