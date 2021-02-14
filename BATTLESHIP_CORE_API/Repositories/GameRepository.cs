using BATTLESHIP_CORE_EF;
using BATTLESHIP_CORE_EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BATTLESHIP_CORE_API.Repositories
{

    public interface IGameRepository : IRepository<Board>
    {
        Board GetGameByID(int id);
        void SetGameStatus(Board board, string Status);

    }

    public class GameRepository : Repository<Board>, IGameRepository
    {
        public GameRepository(DBContext db) : base(db)
        {
        }

        public Board GetGameByID(int GAMES_ID)
        {
            return _db.Board.FirstOrDefault(c => c.GAMES_ID == GAMES_ID);
        }

        public void SetGameStatus(Board board, string Status)
        {
            board.STATUS = Status;
        }


    }


}
