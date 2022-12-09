using Infrastructure.AppConfig.Interfaces;

namespace Infrastructure.AppConfig.Implementations
{
    public class AppConfig : IAppConfig
    {
        public int CacheTime { get; set; }
    }
}
