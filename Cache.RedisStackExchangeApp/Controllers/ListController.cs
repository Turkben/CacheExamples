using Cache.RedisStackExchangeApp.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Xml.Linq;

namespace Cache.RedisStackExchangeApp.Controllers
{
    public class ListController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private string listKey = "names";
        public ListController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDb(1);
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            _database.ListRightPush(listKey, name);
            return RedirectToAction("Index");
        }

        public IActionResult Detail()
        {
            List<string> list = new List<string>();
            if (_database.KeyExists(listKey))
            {
                _database.ListRange(listKey).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });
            }
            
            return View(list);
        }

        public IActionResult Remove(string name)
        {
            _database.ListRemove(listKey, name);
            
            return RedirectToAction("Detail");
        }

    }
}
