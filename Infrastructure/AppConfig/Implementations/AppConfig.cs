using Infrastructure.AppConfig.Interfaces;

namespace Infrastructure.AppConfig.Implementations
{
    public class AppConfig : IAppConfig
    {
        public string BaseUrl { get; set; }
        public int CacheTime { get; set; }
    }
}
