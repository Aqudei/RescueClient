using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views.Helpers
{
    public abstract class PageBase : ViewModelBase
    {
        public abstract void OnShow<T>(T args);
    }
}
