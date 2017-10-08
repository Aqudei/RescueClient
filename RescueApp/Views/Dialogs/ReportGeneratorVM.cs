using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views.Dialogs
{
    public class ReportGeneratorVM : ViewModelBase
    {
        private RescueClient rescueClient;

        public ObservableCollection<Models.Incident> Incidents { get; set; }
            = new ObservableCollection<Models.Incident>();

        public ReportGeneratorVM(RescueClient rescueClient)
        {
            this.rescueClient = rescueClient;
        }

        public void OnLoad()
        {
            rescueClient.GetIncidents((ex, incis) =>
            {
                if (ex == null)
                {
                    foreach (var inci in incis)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            Incidents.Add(inci);
                        });
                    }
                }
            });
        }
    }
}
