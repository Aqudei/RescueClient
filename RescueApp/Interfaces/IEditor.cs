using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Interfaces
{
    public interface IEditor<T>
    {
        void Edit(T item);
    }
}
