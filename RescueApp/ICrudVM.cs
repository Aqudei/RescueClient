using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp
{
    public interface ICrudVM<T>
    {
        RelayCommand CreateItemCommand { get; }
        RelayCommand<T> DeleteItemCommand { get;  }
    }
}
