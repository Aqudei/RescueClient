using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views
{
    public class StatisticsVM : ViewModelBase
    {
        public Models.Statistics Stats { get; set; } = new Models.Statistics();

        public StatisticsVM(RescueClient rescueClient)
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
    }
}
