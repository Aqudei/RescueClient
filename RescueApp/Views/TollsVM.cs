using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using RescueApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views
{
    public class TollsVM : ViewModelBase
    {
        private readonly RescueClient rescueClient;

        public ObservableCollection<Toll> Tolls { get; set; }
            = new ObservableCollection<Toll>();

        public TollsVM(RescueClient rescueClient)
        {
            this.rescueClient = rescueClient;

            MessengerInstance.Register<Messages.UpdateTolls>(this, (ut) =>
            {
                ReadTollChanges();
            });

            ReadTollChanges();
        }

        private void ReadTollChanges()
        {
            Tolls.Clear();
            rescueClient.GetTolls((ex, tolls) =>
            {
                if (ex == null)
                {
                    foreach (var t in tolls)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            Tolls.Add(t);
                        });
                    }
                }
            });
        }
    }
}
