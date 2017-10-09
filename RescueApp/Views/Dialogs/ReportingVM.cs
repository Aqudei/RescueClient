using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views.Dialogs
{
    public class ReportingVM : ViewModelBase

    {
        private readonly RescueClient rescueClient;

        public ObservableCollection<Models.Incident> Incidents { get; set; }
            = new ObservableCollection<Models.Incident>();

        public ReportingVM(RescueClient rescueClient)
        {
            this.rescueClient = rescueClient;
        }

        private bool peopleReport;

        public bool PeopleReport
        {
            get { return peopleReport; }
            set { Set(ref peopleReport, value); }
        }

        private RelayCommand viewReportCommand;

        public RelayCommand ViewReportCommand
        {
            get
            {
                return viewReportCommand ?? (viewReportCommand = new RelayCommand(() =>
                {

                }));
            }
        }

        public void OnLoad()
        {
            Incidents.Clear();
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
