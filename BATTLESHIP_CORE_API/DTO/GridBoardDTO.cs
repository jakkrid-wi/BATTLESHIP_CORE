using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BATTLESHIP_CORE_API.DTO
{
    public class GridBoardDTO
    {
        public BoardATK MyBoard { get; set; }
        public BoardATK MyBoardATK { get; set; }
    }


    public class BoardATK
    {
        public List<LocationDTO> Location { get; set; }
    }
}
