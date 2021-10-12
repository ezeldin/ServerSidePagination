using System.Configuration;

namespace Helpers
{
    public static class Utility
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["BikeStoresConnection"].ConnectionString;
        }
    }
}