using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sm_coding_challenge.Models;
using sm_coding_challenge.Services.DataProvider;

namespace sm_coding_challenge.Controllers
{
    public class HomeController : Controller
    {

        private IDataProvider _dataProvider;
        public HomeController(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Player(string id)
        {
            return Json(_dataProvider.GetPlayerById(id));
        }

        [HttpGet]
        public IActionResult Players(string ids)
        {
            var returnList = (null != ids) ? _dataProvider.GetPlayersById(ids.Split(",")) : new List<PlayerModel>();

            return Json(returnList);
        }

        [HttpGet]
        public IActionResult GetAllPlayers()
        {
            var returnList = _dataProvider.GetPlayers();

            return Json(returnList);
        }

        [HttpGet]
        public IActionResult LatestPlayers(string ids)
        {
            var returnList = (null != ids) ? _dataProvider.GetLatestPlayers(ids.Split(",")) : new List<PlayerModel>();

            return Json(returnList);
        }
        
 
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
