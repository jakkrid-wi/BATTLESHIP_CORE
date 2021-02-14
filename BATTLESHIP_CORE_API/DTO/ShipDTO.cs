using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BATTLESHIP_CORE_API.DTO
{
    public class ShipInstall  
    {
        [Required]
        public int GAME_ID { get; set; }
        public ShipDTO Ship { get; set; }
    }

    public class ShipDTO
    {
        [Required]
        public int SHIP_ID { get; set; }
        public List<LocationDTO> Location { get; set; }
    }

  

}
