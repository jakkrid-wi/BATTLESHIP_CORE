using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BATTLESHIP_CORE_EF.Models
{
    public class ActionMove
    {
        [Key]
        public int ID { get; set; }
        public int GAME_ID { get; set; }
        public int PLAYER { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        /// HIT
        /// MISS
        /// MARK X
        /// </summary>
        public string RESULT { get; set; }

        public Board Board { get; set; }
    }
}
