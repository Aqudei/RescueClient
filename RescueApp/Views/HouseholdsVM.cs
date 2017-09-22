using GalaSoft.MvvmLight;
using RescueApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RescueApp.Views
{
    public class HouseholdsVM : ViewModelBase
    {
        private ObservableCollection<Household> _households
            = new ObservableCollection<Household>();

        public ICollectionView Households => CollectionViewSource.GetDefaultView(_households);

        public HouseholdsVM()
        {

        }
    }
}
