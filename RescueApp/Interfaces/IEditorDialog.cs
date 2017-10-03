using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Interfaces
{
    public interface IEditorDialog<T>
    {
        void Edit(T item);
    }
}
