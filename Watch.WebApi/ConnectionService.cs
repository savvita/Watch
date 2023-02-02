using StackExchange.Redis;

namespace Watch.WebApi
{
    public class ConnectionService
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection;
        public static ConnectionMultiplexer Connection
        {
            get => lazyConnection.Value;
        }

        static ConnectionService()
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(ConfigurationManager.Configuration["RedisUrl"]);
            });
        }
    }
}
