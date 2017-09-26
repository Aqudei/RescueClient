using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using RescueApp.Interfaces;
using RescueApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views
{
    public class CenterSelectorVM : ViewModelBase, IEditor<Center>
    {
        private readonly RescueClient rescueClient;

        private Center _currentCenter;

        public Center CurrentCenter
        {
            get { return _currentCenter; }
            set { Set(ref _currentCenter, value); }
        }

        public CenterSelectorVM(RescueClient rescueClient)
        {
            this.rescueClient = rescueClient;
        }


        ObservableCollection<DownloadPersonModel> _people
            = new ObservableCollection<DownloadPersonModel>();

        public void Edit(Center item)
        {
            CurrentCenter = item;

            rescueClient.GetPeople((ex, ppl) =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    _people.Clear();
                });
                if (ex == null)
                {

                }
            });
        }
    }
}
