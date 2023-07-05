using Cache.RedisStackExchangeApp.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Cache.RedisStackExchangeApp.Controllers
{
    public class StringController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        public StringController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            //var db = _redisService.GetDb(0);
            _database.StringSet("name", "Gökhan");
            _database.StringSet("visitorCount", 100);
            
            return View();
        }

        public IActionResult Detail()
        {
            var value = _database.StringGet("name");
            var value2 = _database.StringGetRange("name", 0, 3);
            var value3 = _database.StringLength("name");

            _database.StringIncrement("visitorCount", 1);
            _database.StringDecrementAsync("visitorCount",10).Wait();
            

            ViewBag.visitorCount = _database.StringGet("visitorCount");

            if(value.HasValue)
            {
                ViewBag.value = value.ToString();
            }


            return View();
        }
    }
}
