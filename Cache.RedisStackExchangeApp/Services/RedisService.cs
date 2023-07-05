using StackExchange.Redis;

namespace Cache.RedisStackExchangeApp.Services
{
    public class RedisService
    {
        //private readonly IConfiguration _configuration;

        private readonly string _redisHost;
        private readonly string _redisPort;

        public IDatabase database { get; set; }

        private ConnectionMultiplexer _redis;
        

        public RedisService(IConfiguration configuration)
        {
            //_configuration = configuration;
            _redisHost = configuration["Redis:Host"];
            _redisPort = configuration["Redis:Port"];

            Connect();
        }

        private void Connect()
        {
            var configString = $"{_redisHost}:{_redisPort}";

            _redis = ConnectionMultiplexer.Connect(configString);
        }

        public IDatabase GetDb(int db)
        {
            return _redis.GetDatabase(db);
        }

    }
}
