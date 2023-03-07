namespace Watch.WebApi
{
    public static class ConfigurationManager
    {
        public static IConfiguration Configuration { get; }
        public static string FileRoot { get; } = @"d:\html_source\React\watchshopmarket\public\images\";
        //public static string FileRoot { get; } = "https://savvita.blob.core.windows.net/files/";
        static ConfigurationManager()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
