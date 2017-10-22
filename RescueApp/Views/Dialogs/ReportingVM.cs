using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Threading;
using RescueApp.Models;
using RescueApp.ViewServices;
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
        private readonly DialogService dialogService;

        public ObservableCollection<Models.Incident> Incidents { get; set; }
            = new ObservableCollection<Models.Incident>();

        private Incident selectedIncident;

        public Incident SelectedIncident
        {
            set { selectedIncident = value; }
        }


        public ReportingVM(RescueClient rescueClient, DialogService dialogService)
        {
            this.rescueClient = rescueClient;
            this.dialogService = dialogService;
        }

        private bool peopleReport = true;

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
                    if (peopleReport)
                    {
                        rescueClient.GetPeopleReport(selectedIncident, (ex, reportData) =>
                        {
                            if (ex == null)
                            {
                                Reports.PeopleReport reportDocument = new Reports.PeopleReport();
                                reportDocument.SetDataSource(reportData);
                                Dictionary<string, object> parames = new Dictionary<string, object>();
                                parames.Add("CalamityName", selectedIncident.IncidentName);

                                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                {
                                    dialogService.ShowReport(reportDocument, parames);
                                });
                            }
                        });
                    }
                    else
                    {
                        rescueClient.GetHousesStatus((ex, statuses) =>
                        {
                            if (ex == null)
                            {
                                Reports.HouseholdReport rptDocumentHouses = new Reports.HouseholdReport();
                                rptDocumentHouses.SetDataSource(statuses);
                                Dictionary<string, object> parames = new Dictionary<string, object>();
                                parames.Add("CalamityName", selectedIncident.IncidentName);
                                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                {
                                    dialogService.ShowReport(rptDocumentHouses, parames);
                                });
                            }
                        });
                    }
                }));
            }
        }


        private RelayCommand vulnerablesReportCommand;

        public RelayCommand VulnerablesReportCommand
        {
            get
            {
                return vulnerablesReportCommand ?? (vulnerablesReportCommand = new RelayCommand(() =>
                {
                    rescueClient.ListInVulnerables((ex, rslt) =>
                    {
                        if (ex == null)
                        {
                            Reports.VulnerablePeopleReport vulnerablePeople = new Reports.VulnerablePeopleReport();
                            vulnerablePeople.SetDataSource(rslt);
                            Dictionary<string, object> parames = new Dictionary<string, object>();
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                dialogService.ShowReport(vulnerablePeople, parames);
                            });
                        }
                    });
                }));
            }

        }

        private RelayCommand houseInDangerReportCommand;

        public RelayCommand HouseInDangerReportCommand
        {
            get
            {
                return houseInDangerReportCommand ?? (houseInDangerReportCommand = new RelayCommand(() =>
                {
                    rescueClient.ListInDangerHouseholds((ex, rslt) =>
                    {
                        if (ex == null)
                        {
                            Reports.HouseholdsInDangerZoneReport houseInDangerRpt = new Reports.HouseholdsInDangerZoneReport();
                            houseInDangerRpt.SetDataSource(rslt);
                            Dictionary<string, object> parames = new Dictionary<string, object>();
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                dialogService.ShowReport(houseInDangerRpt, parames);
                            });
                        }
                    });
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
