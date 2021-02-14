using BATTLESHIP_CORE_API.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BATTLESHIP_CORE_API.Services
{
    public interface IGameService
    {
        Response CreateBoard();
        Response GetShip();
        Response ShipInstall(ShipInstall model);
        Response ShipBotInstall(int GAME_ID,int PLAYER);
        GridBoardDTO GetBoard(int GAME_ID, int PLAYER);
        Response Attack(int GAME_ID, int PLAYER, int x, int y);
        Response Sumary(int GAME_ID);
    }
}
