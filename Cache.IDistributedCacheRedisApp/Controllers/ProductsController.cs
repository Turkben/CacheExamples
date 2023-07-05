using Cache.IDistributedCacheRedisApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace Cache.IDistributedCacheRedisApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
           
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new()
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(1)
            };

            await _distributedCache.SetStringAsync("surname", "turkben", cacheEntryOptions);

            Product product1 = new Product
            {
                Id = 1,
                Name = "Pen1",
                Price = 12.5m
            };

            Product product2 = new Product
            {
                Id = 2,
                Name = "Pen2",
                Price = 12.5m
            };

            string jsonProduct1 = System.Text.Json.JsonSerializer.Serialize(product1);
            string jsonProduct2 = System.Text.Json.JsonSerializer.Serialize(product2);

            byte[] byteProduct1 = Encoding.UTF8.GetBytes(jsonProduct1);
            byte[] byteProduct2 = Encoding.UTF8.GetBytes(jsonProduct2);

            _distributedCache.Set("product:1", byteProduct1);
            _distributedCache.Set("product:2", byteProduct2);

            //await _distributedCache.SetStringAsync("product:1",jsonProduct1, cacheEntryOptions);
            //await _distributedCache.SetStringAsync("product:2", jsonProduct2, cacheEntryOptions);

            return View();
        }

        public async Task<IActionResult> Detail()
        {
            var surname = await _distributedCache.GetStringAsync("surname");
            var jsonProduct1 = await _distributedCache.GetStringAsync("product:1");
            var jsonProduct2 = await _distributedCache.GetStringAsync("product:2");


            // Byte[] byteProduct1 =  _distributedCache.Get("product:1");
            // Byte[] byteProduct2 =  _distributedCache.Get("product:2");

            //string stringProduct1 = Encoding.UTF8.GetString(byteProduct1);
            // string stringProduct2 = Encoding.UTF8.GetString(byteProduct2);

            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                IgnoreNullValues = true
            };

            var product1 = System.Text.Json.JsonSerializer.Deserialize<Product>(jsonProduct1, jsonSerializerOptions);
            var product2 = System.Text.Json.JsonSerializer.Deserialize<Product>(jsonProduct2, jsonSerializerOptions);

            //var settings = new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //    MissingMemberHandling = MissingMemberHandling.Ignore
            //};
            //var product1 = JsonConvert.DeserializeObject<Product>(jsonProduct1, settings);
            //var product2 = JsonConvert.DeserializeObject<Product>(jsonProduct2, settings);




            ViewBag.Surname = surname;
            ViewBag.product1 = product1.Name;
            ViewBag.product2 = product2.Name;
            return View();
        }

        public IActionResult Remove()
        {

            _distributedCache.Remove("surname");
            _distributedCache.Remove("product:1");
            _distributedCache.Remove("product:2");
            return View();
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/default.jpg");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("image", imageByte);

            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] imgByte = _distributedCache.Get("image");
            return File(imgByte, "image/jpg");
        }
    }
}
