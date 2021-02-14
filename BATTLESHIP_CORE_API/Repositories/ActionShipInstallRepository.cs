using BATTLESHIP_CORE_API.DTO;
using BATTLESHIP_CORE_EF;
using BATTLESHIP_CORE_EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BATTLESHIP_CORE_API.Repositories
{

    public interface IActionShipInstallRepository : IRepository<ActionShipInstall>
    {
        IEnumerable<ActionShipInstall> GetShip(int GAME_ID, int PLAYER);
        bool CheckDistance(ShipDTO Ship, int max_distance, int GAME_ID, int player);
        bool CheckOverBoard(ShipDTO Ship, Board board);
        List<ShipDTO> RandomInstallShip(List<Ship> mShip, Board board);
        void AddShip(ShipDTO Ship, Board board, int player);

        bool CheckShipAlready(int GAME_ID, int player, List<Ship> mShip);

        bool CheckDupShip(int GAME_ID, int player, ShipDTO Ship, List<Ship> mShip);

        List<LocationDTO> GetBoard(int GAME_ID, int player);

        string CheckAttack(int GAME_ID, int player, int x, int y);
    }

    public class ActionShipInstallRepository : Repository<ActionShipInstall>, IActionShipInstallRepository
    {
        public ActionShipInstallRepository(DBContext db) : base(db)
        {

        }

        public IEnumerable<ActionShipInstall> GetShip(int GAME_ID, int PLAYER)
        {
            return _db.ActionShipInstall.Where(c => c.GAME_ID == GAME_ID && c.PLAYER == PLAYER).ToList();
        }

        public bool CheckDistance(ShipDTO Ship, int max_distance, int GAME_ID, int player)
        {
            var ship1 = _db.ActionShipInstall.Where(c => c.GAME_ID == GAME_ID && c.PLAYER == player).ToList();
            foreach (var p1 in ship1)
            {
                foreach (var p2 in Ship.Location)
                {
                    int distance = Convert.ToInt16(Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2)));
                    if (distance <= max_distance)
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        public bool CheckOverBoard(ShipDTO Ship, Board board)
        {
            foreach (var p in Ship.Location)
            {
                if (!Enumerable.Range(1, board.SIZE_X).Contains(p.X) || !Enumerable.Range(1, board.SIZE_Y).Contains(p.Y)) return true;
            }
            return false;
        }

        public List<ShipDTO> RandomInstallShip(List<Ship> mShip, Board board)
        {
            var ship = new List<ShipDTO>();
            foreach (var item in mShip)
            {
                var pointX = new Random().Next(1, board.SIZE_X);
                var pointY = new Random().Next(1, board.SIZE_Y);

                if (item.SIZE_X < item.SIZE_Y)
                {
                    ship.Add(new ShipDTO
                    {
                        SHIP_ID = item.SHIP_ID,
                        Location = new List<LocationDTO>
                        {
                            new LocationDTO { X = pointX ,Y = pointY },
                            new LocationDTO { X = pointX ,Y =pointY + 1},
                        }
                    });
                }
                else
                {
                    var firstX = ship.FirstOrDefault().Location.FirstOrDefault().X;
                    do
                    {
                        pointX = new Random().Next(1, board.SIZE_X);

                    } while (pointX == firstX || pointX + 1 == firstX || pointX - 1 == firstX);



                    ship.Add(new ShipDTO
                    {
                        SHIP_ID = item.SHIP_ID,
                        Location = new List<LocationDTO>
                        {
                            new LocationDTO { X = pointX ,Y = pointY },
                            new LocationDTO { X = pointX + 1 ,Y =pointY},
                        }
                    });
                }
            }

            return ship;
        }

        public void AddShip(ShipDTO Ship, Board board, int player)
        {
            var row = Ship.Location.Select(s => new ActionShipInstall
            {
                GAME_ID = board.GAMES_ID,
                SHIP_ID = Ship.SHIP_ID,
                PLAYER = player,
                X = s.X,
                Y = s.Y
            });
            _db.ActionShipInstall.AddRange(row);
        }

        public bool CheckShipAlready(int GAME_ID, int player, List<Ship> mShip)
        {
            var ship = _db.ActionShipInstall.Where(c => c.GAME_ID == GAME_ID && c.PLAYER == player).ToList();

            if (mShip.Count == ship.Count && mShip.Count != 0) return true;
            else return false;

        }

        public bool CheckDupShip(int GAME_ID, int player, ShipDTO Ship, List<Ship> mShip)
        {
            return _db.ActionShipInstall.Any(c => c.GAME_ID == GAME_ID && c.PLAYER == player && c.SHIP_ID == Ship.SHIP_ID);
        }

        public List<LocationDTO> GetBoard(int GAME_ID, int player)
        {
            var rs = _db.ActionShipInstall.Where(c => c.GAME_ID == GAME_ID && c.PLAYER == player).Select(s => new LocationDTO
            {
                SHIP_ID = s.SHIP_ID,
                X = s.X,
                Y = s.Y,
                RESULT = s.IS_HIT == true ? GameCodes.Fire.HIT : s.IS_HIT == null ? null : GameCodes.Fire.MISS
            }).ToList();


            foreach (var item in rs)
            {
                item.RESULT = CheckMakeX(GAME_ID, player, item.SHIP_ID) ? GameCodes.Fire.MARKX : item.RESULT;
            }


            return rs;
        }

        private bool CheckMakeX(int GAME_ID, int player, int SHIP_ID)
        {
            return _db.ActionShipInstall.Any(c => c.GAME_ID == GAME_ID && c.PLAYER == player && c.SHIP_ID == SHIP_ID && c.IS_HIT == true);
        }

        public string CheckAttack(int GAME_ID, int player, int x, int y)
        {
            var ship = _db.ActionShipInstall.Where(c => c.GAME_ID == GAME_ID && c.PLAYER != player).ToList();
            var hit = ship.FirstOrDefault(c => c.X == x && c.Y == y);
            if (hit != null)
            {
                hit.IS_HIT = true;
                _db.SaveChanges();

                var markx = ship.Where(c => c.SHIP_ID == hit.SHIP_ID).All(c => c.IS_HIT == true);

                return markx == true ? GameCodes.Fire.MARKX : GameCodes.Fire.HIT;
            }
            else
            {
                return GameCodes.Fire.MISS;
            }
        }




    }


}
