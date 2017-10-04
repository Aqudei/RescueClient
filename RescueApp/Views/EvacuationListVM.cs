using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Threading;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Maps.MapControl.WPF;
using RescueApp.Interfaces;
using RescueApp.Messages;
using RescueApp.Models;
using RescueApp.Views.Helpers;
using RescueApp.ViewServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RescueApp.Views
{
    public class EvacuationListVM : PageBase, ICrudVM<Center>, INavigable
    {
        public string Title { get; set; } = "Evacuation Centers";

        public ObservableCollection<Center> Centers { get; set; }
            = new ObservableCollection<Center>();

        public ICollectionView CentersCollectionView
            => CollectionViewSource.GetDefaultView(Centers);

        public EvacuationListVM(RescueClient client, DialogService dialogService,
            IDialogCoordinator dialogCoordinator)
        {
            _rescueClient = client;
            this.dialogService = dialogService;
            this.dialogCoordinator = dialogCoordinator;
            if (IsInDesignModeStatic)
            {
                Centers.Add(new Center
                {
                    CenterName = "PASALUBONG CENTER",
                    Address = "Sitio Merkado, Catarman, Manila",
                    Limit = 300
                });

                Centers.Add(new Center
                {
                    CenterName = "PASALUBONG CENTER",
                    Address = "Sitio Merkado, Catarman, Manila",
                    Limit = 300
                });

                Centers.Add(new Center
                {
                    CenterName = "PASALUBONG CENTER",
                    Address = "Sitio Merkado, Catarman, Manila",
                    Limit = 300
                });

                Centers.Add(new Center
                {
                    CenterName = "PASALUBONG CENTER",
                    Address = "Sitio Merkado, Catarman, Manila",
                    Limit = 300
                });
            }
            else
            {
                MessengerInstance.Register<Messages.AddEditResultMessage<Center>>(this, (rslt) =>
                {
                    if (rslt.Error == null)
                    {
                        var oldCenter = Centers.FirstOrDefault(c => c.id == rslt.Entity.id);

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            if (oldCenter != null)
                                Centers.Remove(oldCenter);
                            Centers.Add(rslt.Entity);
                        });

                        MessengerInstance.Send(default(StatsChangedMessage));
                    }
                });
                CentersCollectionView.CurrentChanged += CentersCollectionView_CurrentChanged;
            }
        }

        private void CentersCollectionView_CurrentChanged(object sender, EventArgs e)
        {
            var current = CentersCollectionView.CurrentItem as Center;
            if (current != null)
                MessengerInstance.Send(new NewCenterForMapMessage
                {
                    Location = new Location(current.Latitude, current.Longitude)
                });
        }

        private void LoadEvacuationCenters()
        {
            _rescueClient.GetCenters((ex, rslt) =>
            {
                if (ex == null)
                {
                    foreach (var item in rslt)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            Centers.Add(item);
                        });
                    }
                }
            });
        }

        //public override void OnShow<T>(T args)
        //{ }

        public void OnNavigated()
        {
            Centers.Clear();
            LoadEvacuationCenters();
        }

        private readonly RescueClient _rescueClient;
        private readonly DialogService dialogService;
        private readonly IDialogCoordinator dialogCoordinator;
        private RelayCommand<Center> _deleteItemCommand;
        public RelayCommand<Center> DeleteItemCommand
        {
            get
            {
                return _deleteItemCommand ?? (_deleteItemCommand = new RelayCommand<Center>(async (item) =>
                {
                    //var yesNo = MessageBox.Show("Are you sure you want to delete "
                    //    + item.CenterName + "?", "CONFIRM DELETE", MessageBoxButton.YesNo);

                    var yesNo = await dialogCoordinator.ShowMessageAsync(this,
                        "CONFIRM DELETE", "Are you sure you want to delete "
                        + item.CenterName + "?", MessageDialogStyle.AffirmativeAndNegative);

                    if (yesNo == MessageDialogResult.Affirmative)
                    {
                        _rescueClient.DeleteCenter(item.id, ex =>
                        {
                            if (ex == null)
                            {
                                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                {
                                    Centers.Remove(item);
                                });
                                MessengerInstance.Send(default(StatsChangedMessage));
                            }
                        });
                    }
                }));
            }
        }

        public RelayCommand CreateItemCommand => new RelayCommand(() =>
        {
            dialogService.ShowDialog("AddEditEvacuation");
        });

        public RelayCommand<Center> EditItemCommand => new RelayCommand<Center>(c =>
        {
            dialogService.ShowDialog<Center>("AddEditEvacuation", c);
        });

        public RelayCommand<Center> CenterAssignmentCommand => new RelayCommand<Center>((c) =>
        {
            dialogService.ShowDialog("CenterSelector", c);
        });
    }
}
