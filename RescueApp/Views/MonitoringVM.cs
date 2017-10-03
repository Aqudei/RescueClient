using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using RescueApp.Interfaces;
using RescueApp.Misc;
using RescueApp.Models;
using RescueApp.Views.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RescueApp.Views
{
    public class MonitoringVM : PageBase, INavigable
    {
        private readonly RescueClient rescueClient;
        private readonly SMSListener smsListener;

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


        public MonitoringVM(RescueClient rescueClient, SMSListener smsListener)
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
            this.smsListener = smsListener;
            smsListener.NewMessageReceived += (s, e) =>
            {
                rescueClient.CheckIn(e.CheckInInfo.Id, (ex, rslt) =>
                {
                    if (ex == null)
                    {
                        var _ms = _monitoringSummaries.Where(ms => ms.center.Id == rslt.center.Id).FirstOrDefault();
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            if (_ms != null)
                            {
                                _monitoringSummaries.Remove(_ms);
                            }
                            _monitoringSummaries.Add(rslt);
                        });
                    }
                    else
                    {
                        Debug.WriteLine(ex.Message);
                    }
                });
            };
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

        public override void Cleanup()
        {
            smsListener.Terminate();
        }
    }
}
