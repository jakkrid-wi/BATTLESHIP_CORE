using BATTLESHIP_CORE_API.DTO;
using BATTLESHIP_CORE_EF;
using BATTLESHIP_CORE_EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BATTLESHIP_CORE_API.Repositories
{

    public interface IActionMoveRepository : IRepository<ActionMove>
    {
        List<LocationDTO> GetAllMove(int GAME_ID, int player);
        bool CheckATKDup(int GAME_ID, int player, int x, int y);
        bool CanATK(int GAME_ID, int player);
    }

    public class ActionMoveRepository : Repository<ActionMove>, IActionMoveRepository
    {
        public ActionMoveRepository(DBContext db) : base(db)
        {
        }

        public List<LocationDTO> GetAllMove(int GAME_ID, int player)
        {
            return _db.ActionMove.Where(c => c.GAME_ID == GAME_ID && c.PLAYER == player).Select(s => new LocationDTO
            {
                X = s.X,
                Y = s.Y,
                RESULT = s.RESULT
            }).ToList();
        }

        public bool CheckATKDup(int GAME_ID, int player, int x, int y)
        {
            return _db.ActionMove.Any(c => c.GAME_ID == GAME_ID && c.PLAYER == player && c.X == x && c.Y == y);
        }


        public bool CanATK(int GAME_ID, int player)
        {
            var last = _db.ActionMove.LastOrDefault(c => c.GAME_ID == GAME_ID);
            return last?.PLAYER == player;
        }

    }


}
