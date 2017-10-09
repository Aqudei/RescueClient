using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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

            this.rescueClient = rescueClient;
            this.smsListener = smsListener;

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

            smsListener.NewMessageReceived += (s, e) =>
            {
                e.CheckInInfo.status = "safe";
                rescueClient.CheckIn(e.CheckInInfo, (ex, monitoringSummary) =>
                {
                    if (ex == null)
                    {
                        UpdateSummary(monitoringSummary);
                    }
                    else
                    {
                        Debug.WriteLine(ex.Message);
                    }
                });
            };
        }

        private void UpdateSummary(MonitoringSummary monitoringSummary)
        {
            var _ms = _monitoringSummaries.Where(ms => ms.center.id == monitoringSummary.center.id).FirstOrDefault();

            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                MonitoringSummary lastCurrent = null;
                if (_ms != null)
                {
                    lastCurrent = SummariesCollectionView.CurrentItem as MonitoringSummary;
                    _monitoringSummaries.Remove(_ms);
                }
                _monitoringSummaries.Add(monitoringSummary);
                if (lastCurrent != null && lastCurrent.center.id == monitoringSummary.center.id)
                {
                    SummariesCollectionView.MoveCurrentTo(monitoringSummary);
                }
            });
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


        public string FilterKeyword
        {
            set
            {
                var len = string.IsNullOrEmpty(value) == true ? 0 : value.Length;

                if (_currentMonitoringInfo != null)
                {
                    if (len >= 3)
                        _notCheckedIn.Filter = (obj) =>
                        {
                            var asPerson = obj as DownloadPersonModel;
                            var found = _currentMonitoringInfo.persons.Where(p => p.id == asPerson.id).Any();
                            var include = !found;
                            include = include && asPerson.FullName.ToLower().Contains(value.ToLower());
                            return include;
                        };
                    else
                        _notCheckedIn.Filter = (obj) =>
                        {
                            var asPerson = obj as DownloadPersonModel;
                            var found = _currentMonitoringInfo.persons.Where(p => p.id == asPerson.id).Any();
                            var include = !found;
                            return include;
                        };
                }

            }
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
                        var include = !found;
                        return include;
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

                    _checkedIn = CollectionViewSource.GetDefaultView(_currentMonitoringInfo.persons);

                    //_checkedIn.Filter = (obj) =>
                    //{
                    //    var asPerson = obj as DownloadPersonModel;
                    //    var found = _currentMonitoringInfo.persons.Where(p => p.id == asPerson.id).Any();
                    //    return found;
                    //};

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

        private RelayCommand<DownloadPersonModel> setSafeCommand;
        public RelayCommand<DownloadPersonModel> SetSafeCommand
        {
            get
            {
                return setSafeCommand ?? (setSafeCommand = new RelayCommand<DownloadPersonModel>((arg) =>
                {
                    var chkinfo = new CheckInInfo
                    {
                        Id = arg.id,
                        scope = "self",
                        status = "safe"
                    };
                    rescueClient.CheckIn(chkinfo, (ex, sum) =>
                    {
                        if (ex == null)
                        {
                            UpdateSummary(sum);
                        }
                        else
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    });
                }));
            }

        }

        private RelayCommand<DownloadPersonModel> setDeadCommand;

        public RelayCommand<DownloadPersonModel> SetDeadCommand
        {
            get
            {
                return setDeadCommand ?? (setDeadCommand = new RelayCommand<DownloadPersonModel>((person) =>
                {
                    var chkinfo = new CheckInInfo
                    {
                        Id = person.id,
                        scope = "self",
                        status = "dead"
                    };
                    rescueClient.CheckIn(chkinfo, (ex, sum) =>
                    {
                        if (ex == null)
                        {
                            UpdateSummary(sum);
                        }
                        else
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    });
                }));
            }
        }

        private RelayCommand<DownloadPersonModel> setMissingCommand;
        public RelayCommand<DownloadPersonModel> SetMissingCommand
        {
            get
            {
                return setMissingCommand ?? (setMissingCommand = new RelayCommand<DownloadPersonModel>((person) =>
                {
                    var chkinfo = new CheckInInfo
                    {
                        Id = person.id,
                        scope = "self",
                        status = "missing"
                    };
                    rescueClient.CheckIn(chkinfo, (ex, sum) =>
                    {
                        if (ex == null)
                        {
                            UpdateSummary(sum);
                        }
                        else
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    });
                }));
            }
        }

        private RelayCommand<DownloadPersonModel> setInjuredCommand;
        public RelayCommand<DownloadPersonModel> SetInjuredCommand
        {
            get
            {
                return setInjuredCommand ?? (setInjuredCommand = new RelayCommand<DownloadPersonModel>((person) =>
                {
                    var chkinfo = new CheckInInfo
                    {
                        Id = person.id,
                        scope = "self",
                        status = "injured"
                    };
                    rescueClient.CheckIn(chkinfo, (ex, sum) =>
                    {
                        if (ex == null)
                        {
                            UpdateSummary(sum);
                        }
                        else
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    });
                }));
            }
        }
    }
}
