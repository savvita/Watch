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
                if(ConfigurationManager.Configuration["RedisUrl"] == null)
                {
                    throw new ArgumentNullException();
                }
                return ConnectionMultiplexer.Connect(ConfigurationManager.Configuration["RedisUrl"]!);
            });
        }
    }
}
