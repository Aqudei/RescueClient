using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
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
        public String Title { get; set; } = "Households";

        public ObservableCollection<Household> _households
            = new ObservableCollection<Household>();
        private readonly RescueClient rescueClient;

        private ICollectionView _householdsView;
        public ICollectionView Households
        {
            get
            {
                return _householdsView ?? (_householdsView = CollectionViewSource.GetDefaultView(_households));
            }
        }

        public HouseholdsVM(RescueClient rescueClient)
        {
            this.rescueClient = rescueClient;

            rescueClient.GetHouseholds((ex, hous) =>
            {
                if (ex == null)
                {
                    foreach (var item in hous)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            _households.Add(item);
                        });
                    }
                }
            });
        }
    }
}
