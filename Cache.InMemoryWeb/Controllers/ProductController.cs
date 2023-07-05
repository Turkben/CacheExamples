using Cache.InMemoryWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Cache.InMemoryWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

      
        public IActionResult Index()
        {
            if (!_memoryCache.TryGetValue("Time",out string timeCache))
            {

                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions();

                cacheOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
                cacheOptions.SlidingExpiration = TimeSpan.FromSeconds(10);

                //cacheOptions.AbsoluteExpiration = DateTime.Now.AddSeconds(10);
                cacheOptions.Priority=CacheItemPriority.Normal;
                cacheOptions.RegisterPostEvictionCallback((key, value, reason,state) =>
                {
                    _memoryCache.Set("callback", $"key:{key}==>value:{value} ==> reason:{reason}");

                });

                Product product1 = new Product()
                {
                    Id=1,
                    Name="Pen1",
                    Price=12.5m,
                };

                _memoryCache.Set("product1", product1);

                _memoryCache.Set<string>("Time", DateTime.Now.ToString(), cacheOptions);
            }

            //_memoryCache.GetOrCreate("Time", x =>
            //{
            //    return DateTime.Now.ToString();
            //});

            return View();
        }

        public IActionResult Detail()
        {

            _memoryCache.TryGetValue("Time", out string timeCache);
            _memoryCache.TryGetValue("callback", out string calback);
            _memoryCache.TryGetValue("product1", out Product product1);
            var pro1 = _memoryCache.Get<Product>("product1");

            ViewBag.Time = timeCache;
            ViewBag.callback = calback;
            ViewBag.product1 = product1.Name;
            //var data = _memoryCache.Get<string>("Time");
            //ViewBag.Time = data;
            return View();  
        }

        public IActionResult Delete()
        {
           
            _memoryCache.Remove("Time");
            return View();
        }
    }
}
