namespace Infrastructure.AppConfig.Interfaces
{
    public interface IAppConfig
    {
        public string BaseUrl { get; set; }
        public int CacheTime { get; set; }
    }
}
