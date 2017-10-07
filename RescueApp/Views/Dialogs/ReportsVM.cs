using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using RescueApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views.Dialogs
{
    public class ReportsVM : ViewModelBase
    {
        private readonly RescueClient rescueClient;
        public ObservableCollection<Models.Incident> Incidents
            => new ObservableCollection<Models.Incident>();

        public Incident SelectedIncident { get; set; }

        public ReportsVM(RescueClient rescueClient)
        {
            this.rescueClient = rescueClient;
        }

        public void OnShown()
        {
            Incidents.Clear();
            rescueClient.GetIncidents((ex, incidents) =>
            {
                if (ex == null)
                {
                    foreach (var i in incidents)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            Incidents.Add(i);
                        });
                    }
                }
            });
        }
    }
}
