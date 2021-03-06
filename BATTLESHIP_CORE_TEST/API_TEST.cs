﻿using BATTLESHIP_CORE_API;
using BATTLESHIP_CORE_API.DTO;
using BATTLESHIP_CORE_API.Services;
using BATTLESHIP_CORE_API.UnitOfWorks;
using BATTLESHIP_CORE_EF.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BATTLESHIP_CORE_TEST
{
    public class API_TEST
    {
        [Fact]
        public void CreateBoard()
        {
            // Arrange
            var dbContext = DbContextMocker.GetDbContext(nameof(CreateBoard));
            var service = new GameService(new UnitOfWork(dbContext));

            // Act
            var result = service.CreateBoard();
            var board = ((Board)result.Data);

            // Assert
            Assert.Equal(GameCodes.Status.INSTALL, board.STATUS);
        }

        [Fact]
        public void ShipInstall_Ship1()
        {
            // Arrange
            var dbContext = DbContextMocker.GetDbContext(nameof(ShipInstall_Ship1));
            var service = new GameService(new UnitOfWork(dbContext));

            // Act
            var board = (Board)service.CreateBoard().Data;
            var result = service.ShipInstall(new ShipInstall
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

            // Assert
            Assert.Equal(GameCodes.Status.INSTALL, ((Board)result.Data).STATUS);
        }

        [Fact]
        public void ShipInstall_Ship12_34()
        {
            // Arrange
            var dbContext = DbContextMocker.GetDbContext(nameof(ShipInstall_Ship12_34));
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
                        new LocationDTO{ X =4, Y =3},
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
                        new LocationDTO{ X =4, Y =4},
                    }
                }
            });
            Assert.Equal(GameCodes.Status.READY, ((Board)rsShip22.Data).STATUS);


            var MyBoard = service.GetBoard(board.GAMES_ID, 1);
            Assert.Equal(4, MyBoard.MyBoard.Location.Count);

            var BotBoard = service.GetBoard(board.GAMES_ID, 2);
            Assert.Equal(4, BotBoard.MyBoard.Location.Count);
         

        }

        [Fact]
        public void ShipInstall_Ship12_BotRandom()
        {
            // Arrange
            var dbContext = DbContextMocker.GetDbContext(nameof(ShipInstall_Ship1));
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
                        new LocationDTO{ X =4, Y =3},
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
          

        }

        [Fact]
        public void ShipInstall_Ship12_Distance()
        {
            // Arrange
            var dbContext = DbContextMocker.GetDbContext(nameof(ShipInstall_Ship12_Distance));
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
                        new LocationDTO{ X = 2, Y =3},
                        new LocationDTO{ X = 3, Y =3},
                    }
                }
            });
            Assert.Equal("วางเรือหางกัน 1 ช่อง", rsShip2.Message);
        }

        [Fact]
        public void ShipInstall_Ship12_Duplicate()
        {
            // Arrange
            var dbContext = DbContextMocker.GetDbContext(nameof(ShipInstall_Ship12_Duplicate));
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
                    SHIP_ID = 1,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 3, Y =3},
                        new LocationDTO{ X = 4, Y =3},
                    }
                }
            });
            Assert.Equal("SHIP_ID Duplicate!", rsShip2.Message);
        }

        [Fact]
        public void ShipInstall_Ship12_OverBoard()
        {
            // Arrange
            var dbContext = DbContextMocker.GetDbContext(nameof(ShipInstall_Ship12_OverBoard));
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
                        new LocationDTO{ X = 1, Y =5},
                        new LocationDTO{ X = 1, Y =6},
                    }
                }
            });
            Assert.Equal("วางเรือเกินพื้นที่ 5*5 ช่อง", rsShip1.Message);

        }

        [Fact]
        public void ShipInstall_Ship12_NoGameID()
        {
            // Arrange
            var dbContext = DbContextMocker.GetDbContext(nameof(ShipInstall_Ship12_Duplicate));
            var service = new GameService(new UnitOfWork(dbContext));

            // Act
            var rsShip1 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = 99,
                PLAYER = 1,
                Ship = new ShipDTO
                {
                    SHIP_ID = 1,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 1, Y =2},
                        new LocationDTO{ X = 1, Y =3},
                    }
                }
            });
            Assert.Equal("ยังไม่สร้าง board", rsShip1.Message);

        }

        [Fact]
        public void ShipInstall_Ship12_OverShip()
        {
            // Arrange
            var dbContext = DbContextMocker.GetDbContext(nameof(ShipInstall_Ship12_OverShip));
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


            var rsShip3 = service.ShipInstall(new ShipInstall
            {
                GAME_ID = board.GAMES_ID,
                PLAYER = 1,
                Ship = new ShipDTO
                {
                    SHIP_ID = 1,
                    Location = new List<LocationDTO>
                    {
                        new LocationDTO{ X = 3, Y =3},
                        new LocationDTO{ X = 4, Y =3},
                    }
                }
            });
            Assert.Equal("เกมนี้ไม่สามารถวางเรือได้แล้ว", rsShip3.Message);
        }


      
    }
}