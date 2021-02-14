using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BATTLESHIP_CORE_EF.Models
{
    public class Ship
    {
        [Key]
        public int SHIP_ID { get; set; }
        public string SHIP_NAME { get; set; }
        public int SIZE_X { get; set; }
        public int SIZE_Y { get; set; }

        public ICollection<ActionShipInstall> ActionShipInstall { get; set; }
    }
}
