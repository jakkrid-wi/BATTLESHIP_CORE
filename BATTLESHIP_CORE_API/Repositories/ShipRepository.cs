using BATTLESHIP_CORE_EF;
using BATTLESHIP_CORE_EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BATTLESHIP_CORE_API.Repositories
{

    public interface IShipRepository : IRepository<Ship>
    {
        IEnumerable<Ship> GetAllShip();
        void InsertMasterShip();
    }

    public class ShipRepository : Repository<Ship>, IShipRepository
    {
        public ShipRepository(DBContext db) : base(db)
        {
        }

        public IEnumerable<Ship> GetAllShip()
        {
            return _db.Ship.ToList();
        }


        public void InsertMasterShip()
        {
            var ship = _db.Ship.ToList();
            if (ship.Count == 0)
            {
                _db.Ship.Add(new Ship { SHIP_ID = 1, SHIP_NAME = "AAA", SIZE_X = 1, SIZE_Y = 2 });
                _db.Ship.Add(new Ship { SHIP_ID = 2, SHIP_NAME = "AAA", SIZE_X = 2, SIZE_Y = 1 });
                _db.SaveChanges();
            }
        }
    }


}
