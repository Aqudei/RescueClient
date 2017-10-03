using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Models
{
    public abstract class WithID : ObservableObject
    {
        private int _id;

        public int id
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

    }
}
