using BATTLESHIP_CORE_API.DTO;
using BATTLESHIP_CORE_API.UnitOfWorks;
using BATTLESHIP_CORE_EF.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BATTLESHIP_CORE_API.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        public GameService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Response CreateBoard()
        {
            _unitOfWork.Ships.InsertMasterShip();
            var board = new Board
            {
                SIZE_X = 5,
                SIZE_Y = 5,
                STATUS = GameCodes.Status.INSTALL,
            };
            _unitOfWork.Games.Add(board);
            _unitOfWork.SaveChanges();
            return new Response
            {
                ResponseCode = "0",
                Data = board
            };
        }

        public Response GetShip()
        {
            _unitOfWork.Ships.InsertMasterShip();
            return new Response
            {
                ResponseCode = "0",
                Data = _unitOfWork.Ships.GetAllShip()
            };
        }

        public Response ShipInstall(ShipInstall model)
        {
            var board = _unitOfWork.Games.GetGameByID(model.GAME_ID);
            var mShip = _unitOfWork.Ships.GetAll().ToList();
            if (board == null)
            {
                return new Response
                {
                    ResponseCode = "1",
                    Message = "ยังไม่สร้าง board"
                };
            }
            else if (board.STATUS != GameCodes.Status.INSTALL || _unitOfWork.ActionShipInstalls.CheckShipOver(board.GAMES_ID, model.PLAYER, mShip))
            {
                return new Response
                {
                    ResponseCode = "1",
                    Message = "เกมนี้ไม่สามารถวางเรือได้แล้ว"
                };
            }

            var rShip = model.Ship;
            //var rShip = JsonConvert.DeserializeObject<List<ShipDTO>>(jsonShip);

            if (_unitOfWork.ActionShipInstalls.CheckOverBoard(rShip, board))
            {
                return new Response
                {
                    ResponseCode = "1",
                    Message = "วางเรือเกินพื้นที่ 5*5 ช่อง"
                };
            }

            if (_unitOfWork.ActionShipInstalls.CheckDistance(rShip, 1, model.GAME_ID, model.PLAYER))
            {
                return new Response
                {
                    ResponseCode = "1",
                    Message = "วางเรือหางกัน 1 ช่อง"
                };
            }

            if (_unitOfWork.ActionShipInstalls.CheckDupShip(model.GAME_ID, model.PLAYER, rShip, mShip))
            {
                return new Response
                {
                    ResponseCode = "1",
                    Message = "SHIP_ID Duplicate!"
                };
            }
            _unitOfWork.ActionShipInstalls.AddShip(rShip, board, model.PLAYER);
            _unitOfWork.SaveChanges();

            AllShipOK(board, model.GAME_ID, mShip);


            return new Response
            {
                ResponseCode = "0",
                Data = board
            };
        }


        public Response ShipBotInstall(int GAME_ID, int PLAYER)
        {
            var board = _unitOfWork.Games.GetGameByID(GAME_ID);
            var mShip = _unitOfWork.Ships.GetAll().ToList();
            //bot ship
            var ship_bot = _unitOfWork.ActionShipInstalls.RandomInstallShip(mShip, board);
            foreach (var item in ship_bot)
            {
                if (_unitOfWork.ActionShipInstalls.CheckOverBoard(item, board))
                {
                    return new Response
                    {
                        ResponseCode = "1",
                        Message = "วางเรือเกินพื้นที่ 5*5 ช่อง"
                    };
                }

                if (_unitOfWork.ActionShipInstalls.CheckDistance(item, 1, GAME_ID, PLAYER))
                {
                    return new Response
                    {
                        ResponseCode = "1",
                        Message = "วางเรือหางกัน 1 ช่อง"
                    };
                }

                if (_unitOfWork.ActionShipInstalls.CheckDupShip(GAME_ID, PLAYER, item, mShip))
                {
                    return new Response
                    {
                        ResponseCode = "1",
                        Message = "SHIP_ID Duplicate!"
                    };
                }

                _unitOfWork.ActionShipInstalls.AddShip(item, board, PLAYER);
                _unitOfWork.SaveChanges();
            }

            AllShipOK(board, GAME_ID, mShip);

            return new Response
            {
                ResponseCode = "0",
                Data = board
            };

        }

        public void AllShipOK(Board board, int GAME_ID, List<Ship> mShip)
        {
            if (_unitOfWork.ActionShipInstalls.CheckShipAlready(GAME_ID, mShip))
            {

                _unitOfWork.Games.SetGameStatus(board, GameCodes.Status.READY);
            }
            else
            {
                _unitOfWork.Games.SetGameStatus(board, GameCodes.Status.INSTALL);
            }

            _unitOfWork.SaveChanges();
        }


        public GridBoardDTO GetBoard(int GAME_ID, int PLAYER)
        {
            var mybaord = _unitOfWork.ActionShipInstalls.GetBoard(GAME_ID, PLAYER);
            var moveboard = _unitOfWork.ActionMoves.GetAllMove(GAME_ID, PLAYER);
            return new GridBoardDTO
            {
                MyBoard = new BoardATK { Location = mybaord },
                MyBoardATK = new BoardATK { Location = moveboard }
            };
        }


        public Response Attack(int GAME_ID, int PLAYER, int x, int y)
        {
            if (_unitOfWork.ActionMoves.CanATK(GAME_ID, PLAYER))
            {
                return new Response { ResponseCode = "1", Message = "รอผู้เล่นฝั่งตรงข้ามโจมตีก่อน" };
            }

            if (_unitOfWork.ActionMoves.CheckATKDup(GAME_ID, PLAYER, x, y))
            {
                return new Response { ResponseCode = "1", Message = "ไม่สามารถโจมตีที่เดิมได้" };
            }

            var result_hit = _unitOfWork.ActionShipInstalls.CheckAttack(GAME_ID, PLAYER, x, y);

            _unitOfWork.ActionMoves.Add(new ActionMove { GAME_ID = GAME_ID, PLAYER = PLAYER, X = x, Y = y, RESULT = result_hit });
            _unitOfWork.SaveChanges();

            var ship_enemy = _unitOfWork.ActionShipInstalls.GetShip(GAME_ID, PLAYER == 1 ? 2 : 1);

            if (ship_enemy.All(c => c.IS_HIT == true))
            {
                var baord = _unitOfWork.Games.GetGameByID(GAME_ID);
                baord.STATUS = GameCodes.Status.END;
                _unitOfWork.SaveChanges();
                return new Response { ResponseCode = "0", Message = "Win!", Data = result_hit };
            }

            return new Response
            {
                ResponseCode = "0",
                Data = result_hit
            };
        }


        public Response Sumary(int GAME_ID)
        {
            var game = _unitOfWork.Games.GetGameByID(GAME_ID);
            if (game.STATUS == GameCodes.Status.END)
            {
                var player1_move = _unitOfWork.ActionMoves.GetAllMove(GAME_ID, 1).Count;
                var player2_move = _unitOfWork.ActionMoves.GetAllMove(GAME_ID, 2).Count;
                return new Response { ResponseCode = "0", Data = new { player1_move, player2_move } };
            }
            else
            {
                return new Response { ResponseCode = "1", Message = "เกมยังไม่จบ" };
            }
        }
    }
}
