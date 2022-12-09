using Infrastructure.Service.Interfaces;

namespace Shop.Service.Interfaces.Proxy
{
    public interface IProxyService<T> : IGenericService<T> where T : class
    {
    }
}
