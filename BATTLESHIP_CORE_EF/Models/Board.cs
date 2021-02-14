using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BATTLESHIP_CORE_EF.Models
{
    public class Board
    {
        [Key]
        public int GAMES_ID { get; set; }
        public string STATUS { get; set; }
        public int SIZE_X { get; set; }
        public int SIZE_Y { get; set; }
        public string RESULT_WIN { get; set; }


        public ICollection<ActionMove> ActionMove { get; set; }
        public ICollection<ActionShipInstall> ActionShipInstall { get; set; }

    }
}
