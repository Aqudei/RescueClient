using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTCrud
{
    public interface IRestCrudClient<ToServer, FromServer>
    {
        void Add(ToServer item, Action<Exception, FromServer> callback);
        void List(Action<Exception, IEnumerable<FromServer>> callback);
    }
}
