using Cache.RedisStackExchangeApp.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Cache.RedisStackExchangeApp.Controllers
{
    public class SetController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private string setKey = "setNames";
        public SetController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDb(2);
        }
        public IActionResult Index()
        {
            HashSet<string> names = new HashSet<string>();

            if (_database.KeyExists(setKey))
            {
                _database.SetMembers(setKey).ToList().ForEach(x =>
                {
                    names.Add(x.ToString());
                });
            }
            return View(names);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            _database.KeyExpire(setKey,TimeSpan.FromMinutes(5));
            _database.SetAdd(setKey, name);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(string name)
        {
            _database.SetRemove(setKey, name);

            return RedirectToAction("Index");
        }



    }
}
