using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using RescueApp.Interfaces;
using RescueApp.Models;
using RescueApp.Views.Helpers;
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
    public class MonitoringVM : PageBase, INavigable
    {
        private readonly RescueClient rescueClient;

        public MonitoringInfo CurrentMonitoringInfo { get; set; }

        public String Title { get; } = "Monitoring";

        private ObservableCollection<MonitoringSummary> _monitoringSummaries
            = new ObservableCollection<MonitoringSummary>();

        private ICollectionView _summariesCollectionView;
        public ICollectionView SummariesCollectionView
        {
            get
            {
                _summariesCollectionView = _summariesCollectionView
                  ?? (_summariesCollectionView = CollectionViewSource.GetDefaultView(_monitoringSummaries));

                return _summariesCollectionView;
            }
        }


        public MonitoringVM(RescueClient rescueClient)
        {

            if (IsInDesignMode)
            {
                _monitoringSummaries.Add(new MonitoringSummary
                {
                    center = new Center
                    {
                        CenterName = "Astrodome",
                        Limit = 9
                    },
                    num_evacuees = 4
                });

                _monitoringSummaries.Add(new MonitoringSummary
                {
                    center = new Center
                    {
                        CenterName = "Astrodome2",
                        Limit = 9
                    },
                    num_evacuees = 4
                });

                _monitoringSummaries.Add(new MonitoringSummary
                {
                    center = new Center
                    {
                        CenterName = "Astrodome3",
                        Limit = 9
                    },
                    num_evacuees = 4
                });
            }

            this.rescueClient = rescueClient;
            MessengerInstance.Register<Messages.NewCheckInMessage>(this, (m) =>
            {
               
            });
        }

        //public override void OnShow<T>(T args)
        //{

        //}

        public void OnNavigated()
        {
            _monitoringSummaries.Clear();
            rescueClient.GetMonitoring((ex, summaries) =>
            {
                if (ex == null)
                {
                    foreach (var s in summaries)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            _monitoringSummaries.Add(s);
                        });
                    }
                }
            });
        }


        public MonitoringSummary CurrentMonitoringSummary
        {
            set
            {

            }
        }
    }
}
