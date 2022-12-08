using Infrastructure.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Service.Interfaces.Proxy
{
    public interface IProxyService<T> : IGenericService<T> where T : class
    {
    }
}
