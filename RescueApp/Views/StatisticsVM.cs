using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using RescueApp.Views.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views
{
    public class StatisticsVM : PageBase
    {
        private readonly RescueClient rescueClient;

        public Models.Statistics Stats { get; set; } = new Models.Statistics();

        public StatisticsVM(RescueClient rescueClient)
        {
            this.rescueClient = rescueClient;

            ReadStatChanges();

            MessengerInstance.Register<Messages.StatsChangedMessage>(this, (statChange) =>
            {
                ReadStatChanges();
            });
        }

        private void ReadStatChanges()
        {
            rescueClient.GetStats((ex, stats) =>
            {
                if (ex == null)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        Stats.NumberOfCalamities = stats.NumberOfCalamities;
                        Stats.NumberOfEvacuation = stats.NumberOfEvacuation;
                        Stats.NumberOfHousehold = stats.NumberOfHousehold;
                        Stats.NumberOfPerson = stats.NumberOfPerson;
                    });
                }
            });
        }

        public override void OnShow<T>(T args)
        {
            
        }
    }
}
