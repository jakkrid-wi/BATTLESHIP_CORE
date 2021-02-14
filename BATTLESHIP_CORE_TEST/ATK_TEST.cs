using BATTLESHIP_CORE_API;
using BATTLESHIP_CORE_API.DTO;
using BATTLESHIP_CORE_API.Services;
using BATTLESHIP_CORE_API.UnitOfWorks;
using BATTLESHIP_CORE_EF.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


namespace BATTLESHIP_CORE_TEST
{
    public class ATK_TEST
    {
        [Fact]
        public void Attack_NoTurn()
        {
            // Arrange
            var dbContext = DbContextMocker.GetDbContext(nameof(Attack_NoTurn));
            var service = new GameService(new UnitOfWork(dbContext));

            // Act
            var board = (Board)service.CreateBoard().Data;

            var rsShip1 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 1,
                Ship = new ShipDTO
                {
                    SHIP_ID = 1,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 1, Y =1},
                        new LocationDTO{ X = 1, Y =2},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.INSTALL, ((Board)rsShip1.Data).STATUS);

            var rsShip2 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 1,
                Ship = new ShipDTO
                {
                    SHIP_ID = 2,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 3, Y =3},
                        new LocationDTO{ X = 4, Y =3},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.INSTALL, ((Board)rsShip2.Data).STATUS);

            //Random Bot Ship
            var rsShipBot = service.ShipBotInstall(board.GAMES_ID, 2);
            Assert.Equal(GameCodes.Status.READY, ((Board)rsShipBot.Data).STATUS);

            var MyBoard = service.GetBoard(board.GAMES_ID, 1);
            Assert.Equal(4, MyBoard.MyBoard.Location.Count);

            var BotBoard = service.GetBoard(board.GAMES_ID, 2);
            Assert.Equal(4, BotBoard.MyBoard.Location.Count);


            //play
            Response hit;
            hit = service.Attack(board.GAMES_ID, 2, 3, 3);
            Assert.Equal(GameCodes.Fire.HIT, hit.Data);

            hit = service.Attack(board.GAMES_ID, 2, 4, 3);
            Assert.Equal("รอผู้เล่นฝั่งตรงข้ามโจมตีก่อน", hit.Message);
        }

        [Fact]
        public void Attack_Dup()
        {
            // Arrange
            var dbContext = DbContextMocker.GetDbContext(nameof(Attack_Dup));
            var service = new GameService(new UnitOfWork(dbContext));

            // Act
            var board = (Board)service.CreateBoard().Data;

            var rsShip1 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 1,
                Ship = new ShipDTO
                {
                    SHIP_ID = 1,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 1, Y =1},
                        new LocationDTO{ X = 1, Y =2},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.INSTALL, ((Board)rsShip1.Data).STATUS);

            var rsShip2 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 1,
                Ship = new ShipDTO
                {
                    SHIP_ID = 2,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 3, Y =3},
                        new LocationDTO{ X = 4, Y =3},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.INSTALL, ((Board)rsShip2.Data).STATUS);

            //player2 ship1
            var rsShip21 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 2,
                Ship = new ShipDTO
                {
                    SHIP_ID = 1,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 1, Y =1},
                        new LocationDTO{ X =1, Y =2},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.INSTALL, ((Board)rsShip21.Data).STATUS);

            var rsShip22 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 2,
                Ship = new ShipDTO
                {
                    SHIP_ID = 2,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 3, Y =4},
                        new LocationDTO{ X = 4, Y =4},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.READY, ((Board)rsShip22.Data).STATUS);

            var MyBoard = service.GetBoard(board.GAMES_ID, 1);
            Assert.Equal(4, MyBoard.MyBoard.Location.Count);

            var BotBoard = service.GetBoard(board.GAMES_ID, 2);
            Assert.Equal(4, BotBoard.MyBoard.Location.Count);


            //play
            Response hit;
            hit = service.Attack(board.GAMES_ID, 1, 3, 4);
            Assert.Equal(GameCodes.Fire.HIT, hit.Data);


            hit = service.Attack(board.GAMES_ID, 2, 3, 3);
            Assert.Equal(GameCodes.Fire.HIT, hit.Data);


            hit = service.Attack(board.GAMES_ID, 1, 3, 4);
            Assert.Equal("ไม่สามารถโจมตีที่เดิมได้", hit.Message);



        }


        [Fact]
        public void Attack_Miss()
        {
            // Arrange
            var dbContext = DbContextMocker.GetDbContext(nameof(Attack_Miss));
            var service = new GameService(new UnitOfWork(dbContext));

            // Act
            var board = (Board)service.CreateBoard().Data;

            var rsShip1 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 1,
                Ship = new ShipDTO
                {
                    SHIP_ID = 1,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 1, Y =1},
                        new LocationDTO{ X = 1, Y =2},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.INSTALL, ((Board)rsShip1.Data).STATUS);

            var rsShip2 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 1,
                Ship = new ShipDTO
                {
                    SHIP_ID = 2,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 3, Y =3},
                        new LocationDTO{ X = 4, Y =3},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.INSTALL, ((Board)rsShip2.Data).STATUS);

            //player2 ship1
            var rsShip21 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 2,
                Ship = new ShipDTO
                {
                    SHIP_ID = 1,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 1, Y =1},
                        new LocationDTO{ X =1, Y =2},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.INSTALL, ((Board)rsShip21.Data).STATUS);

            var rsShip22 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 2,
                Ship = new ShipDTO
                {
                    SHIP_ID = 2,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 3, Y =4},
                        new LocationDTO{ X = 4, Y =4},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.READY, ((Board)rsShip22.Data).STATUS);

            var MyBoard = service.GetBoard(board.GAMES_ID, 1);
            Assert.Equal(4, MyBoard.MyBoard.Location.Count);

            var BotBoard = service.GetBoard(board.GAMES_ID, 2);
            Assert.Equal(4, BotBoard.MyBoard.Location.Count);


            //play
            Response hit;
            hit = service.Attack(board.GAMES_ID, 1, 3, 4);
            Assert.Equal(GameCodes.Fire.HIT, hit.Data);

            hit = service.Attack(board.GAMES_ID, 2, 3, 3);
            Assert.Equal(GameCodes.Fire.HIT, hit.Data);

            hit = service.Attack(board.GAMES_ID, 1, 5, 1);
            Assert.Equal(GameCodes.Fire.MISS, hit.Data);


        }


        [Fact]
        public void Attack_MARK_X()
        {
            // Arrange
            var dbContext = DbContextMocker.GetDbContext(nameof(Attack_MARK_X));
            var service = new GameService(new UnitOfWork(dbContext));

            // Act
            var board = (Board)service.CreateBoard().Data;

            var rsShip1 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 1,
                Ship = new ShipDTO
                {
                    SHIP_ID = 1,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 1, Y =1},
                        new LocationDTO{ X = 1, Y =2},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.INSTALL, ((Board)rsShip1.Data).STATUS);

            var rsShip2 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 1,
                Ship = new ShipDTO
                {
                    SHIP_ID = 2,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 3, Y =3},
                        new LocationDTO{ X = 4, Y =3},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.INSTALL, ((Board)rsShip2.Data).STATUS);

            //player2 ship1
            var rsShip21 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 2,
                Ship = new ShipDTO
                {
                    SHIP_ID = 1,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 1, Y =1},
                        new LocationDTO{ X =1, Y =2},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.INSTALL, ((Board)rsShip21.Data).STATUS);

            var rsShip22 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 2,
                Ship = new ShipDTO
                {
                    SHIP_ID = 2,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 3, Y =4},
                        new LocationDTO{ X = 4, Y =4},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.READY, ((Board)rsShip22.Data).STATUS);

            var MyBoard = service.GetBoard(board.GAMES_ID, 1);
            Assert.Equal(4, MyBoard.MyBoard.Location.Count);

            var BotBoard = service.GetBoard(board.GAMES_ID, 2);
            Assert.Equal(4, BotBoard.MyBoard.Location.Count);


            //play
            Response hit;
            hit = service.Attack(board.GAMES_ID, 1, 3, 4);
            Assert.Equal(GameCodes.Fire.HIT, hit.Data);

            hit = service.Attack(board.GAMES_ID, 2, 3, 3);
            Assert.Equal(GameCodes.Fire.HIT, hit.Data);

            hit = service.Attack(board.GAMES_ID, 1, 4, 4);
            Assert.Equal(GameCodes.Fire.MARKX, hit.Data);


        }

        [Fact]
        public void Attack_Win()
        {
            // Arrange
            var dbContext = DbContextMocker.GetDbContext(nameof(Attack_Win));
            var service = new GameService(new UnitOfWork(dbContext));

            // Act
            var board = (Board)service.CreateBoard().Data;

            var rsShip1 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 1,
                Ship = new ShipDTO
                {
                    SHIP_ID = 1,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 1, Y =1},
                        new LocationDTO{ X = 1, Y =2},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.INSTALL, ((Board)rsShip1.Data).STATUS);

            var rsShip2 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 1,
                Ship = new ShipDTO
                {
                    SHIP_ID = 2,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 3, Y =3},
                        new LocationDTO{ X = 4, Y =3},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.INSTALL, ((Board)rsShip2.Data).STATUS);

            //player2 ship1
            var rsShip21 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 2,
                Ship = new ShipDTO
                {
                    SHIP_ID = 1,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 1, Y =1},
                        new LocationDTO{ X =1, Y =2},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.INSTALL, ((Board)rsShip21.Data).STATUS);

            var rsShip22 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 2,
                Ship = new ShipDTO
                {
                    SHIP_ID = 2,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 3, Y =4},
                        new LocationDTO{ X = 4, Y =4},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.READY, ((Board)rsShip22.Data).STATUS);

            var MyBoard = service.GetBoard(board.GAMES_ID, 1);
            Assert.Equal(4, MyBoard.MyBoard.Location.Count);

            var BotBoard = service.GetBoard(board.GAMES_ID, 2);
            Assert.Equal(4, BotBoard.MyBoard.Location.Count);


            //play
            Response hit;
            hit = service.Attack(board.GAMES_ID, 1, 3, 4);
            Assert.Equal(GameCodes.Fire.HIT, hit.Data);

            hit = service.Attack(board.GAMES_ID, 2, 3, 3);
            Assert.Equal(GameCodes.Fire.HIT, hit.Data);

            hit = service.Attack(board.GAMES_ID, 1, 4, 4);
            Assert.Equal(GameCodes.Fire.MARKX, hit.Data);

            hit = service.Attack(board.GAMES_ID, 2, 2, 2);
            Assert.Equal(GameCodes.Fire.MISS, hit.Data);

            hit = service.Attack(board.GAMES_ID, 1, 1, 1);
            Assert.Equal(GameCodes.Fire.HIT, hit.Data);

            hit = service.Attack(board.GAMES_ID, 2, 5, 5);
            Assert.Equal(GameCodes.Fire.MISS, hit.Data);

            hit = service.Attack(board.GAMES_ID, 1, 1, 2);
            Assert.Equal(GameCodes.Fire.MARKX, hit.Data);
            Assert.Equal("Win!", hit.Message);
        }

    }
}
