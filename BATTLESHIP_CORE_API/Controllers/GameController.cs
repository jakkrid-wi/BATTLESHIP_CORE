using BATTLESHIP_CORE_API.DTO;
using BATTLESHIP_CORE_API.Services;
using BATTLESHIP_CORE_API.UnitOfWorks;
using BATTLESHIP_CORE_EF;
using BATTLESHIP_CORE_EF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BATTLESHIP_CORE_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _service;

        public GameController(IGameService service)
        {
            _service = service;
        }

        #region SetUp game & Ship
        [HttpGet]
        public ActionResult CreateBoard()
        {
            return new JsonResult(_service.CreateBoard());
        }

        [HttpGet]
        public ActionResult GetShip()
        {
            return new JsonResult(_service.GetShip());
        }

        [HttpPost]
        public ActionResult ShipInstall([FromBody] ShipInstall model)
        {
            return new JsonResult(_service.ShipInstall(model));
        }
        #endregion


        #region GetBoard
        [HttpPost]
        public ActionResult GetBoard([FromBody] JObject param)
        {
            int GAME_ID = param["GAME_ID"].Value<int>();
            int PLAYER = param["PLAYER"].Value<int>();
            return new JsonResult(_service.GetBoard(GAME_ID, PLAYER));
        }
        #endregion


        #region Move&Action

        [HttpPost]
        public ActionResult Attack([FromBody] JObject param)
        {
            int GAME_ID = param["GAME_ID"].Value<int>();
            int PLAYER = param["PLAYER"].Value<int>();
            int x = param["X"].Value<int>();
            int y = param["Y"].Value<int>();
            return new JsonResult(_service.Attack(GAME_ID, PLAYER, x, y));
        }

        [HttpPost]
        public ActionResult Sumary([FromBody] JObject param)
        {
            int GAME_ID = param["GAME_ID"].Value<int>();
            return new JsonResult(_service.Sumary(GAME_ID));
        }


        #endregion

    }
}
