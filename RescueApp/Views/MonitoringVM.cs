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

        private MonitoringInfo _currentMonitoringInfo;



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

                _summariesCollectionView.SortDescriptions.Add(new SortDescription("center.CenterName",
                    ListSortDirection.Ascending));
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
                        var _ms = _monitoringSummaries.Where(ms => ms.center.id == rslt.center.id).FirstOrDefault();
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            MonitoringSummary lastCurrent = null;
                            if (_ms != null)
                            {
                                lastCurrent = SummariesCollectionView.CurrentItem as MonitoringSummary;
                                _monitoringSummaries.Remove(_ms);
                            }
                            _monitoringSummaries.Add(rslt);
                            if (lastCurrent != null && lastCurrent.center.id == rslt.center.id)
                            {
                                SummariesCollectionView.MoveCurrentTo(rslt);
                            }
                        });
                    }
                    else
                    {
                        Debug.WriteLine(ex.Message);
                    }
                });
            };
        }


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


        private ICollectionView _notCheckedIn;

        public ICollectionView NotCheckedIn
        {
            get
            {
                if (_currentMonitoringInfo != null)
                {
                    _notCheckedIn = new CollectionViewSource { Source = _currentMonitoringInfo.center.members }.View;
                    _notCheckedIn.Filter = (obj) =>
                    {
                        var asPerson = obj as DownloadPersonModel;
                        var found = _currentMonitoringInfo.persons.Where(p => p.id == asPerson.id).Any();
                        return !found;
                    };

                    return _notCheckedIn;
                }

                return default(ICollectionView);
            }
        }

        private ICollectionView _checkedIn;

        public ICollectionView CheckedIn
        {
            get
            {
                if (_currentMonitoringInfo != null)
                {
                    _checkedIn = new CollectionViewSource { Source = _currentMonitoringInfo.center.members }.View;

                    _checkedIn.Filter = (obj) =>
                    {
                        var asPerson = obj as DownloadPersonModel;
                        var found = _currentMonitoringInfo.persons.Where(p => p.id == asPerson.id).Any();
                        return found;
                    };

                    return _checkedIn;
                }

                return default(ICollectionView);
            }
        }

        public MonitoringSummary CurrentMonitoringSummary
        {
            set
            {
                if (value != null)
                    rescueClient.GetMonitoringDetail(value.center.id, (ex, monitoringInfo) =>
                    {
                        if (ex == null)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                _currentMonitoringInfo = monitoringInfo;

                                RaisePropertyChanged(nameof(CheckedIn));
                                RaisePropertyChanged(nameof(NotCheckedIn));





                            });
                        }
                    });
                else
                {
                    _currentMonitoringInfo = null;
                    RaisePropertyChanged(nameof(CheckedIn));
                    RaisePropertyChanged(nameof(NotCheckedIn));
                }
            }
        }

        public override void Cleanup()
        {
            smsListener.Terminate();
        }
    }
}
