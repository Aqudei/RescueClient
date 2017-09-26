using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using RescueApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using GalaSoft.MvvmLight.CommandWpf;
using RescueApp.ViewServices;
using RescueApp.Views.Helpers;

namespace RescueApp.Views
{
    public class HouseholdsVM : PageBase, ICrudVM<DownloadHouseholdModel>
    {
        public String Title { get; set; } = "Households";

        public ObservableCollection<DownloadHouseholdModel> _households
            = new ObservableCollection<DownloadHouseholdModel>();
        private readonly RescueClient rescueClient;

        private ICollectionView _householdsView;
        private readonly DialogService dialogService;

        public ICollectionView Households
        {
            get
            {
                return _householdsView ?? (_householdsView = CollectionViewSource.GetDefaultView(_households));
            }
        }

        public RelayCommand CreateItemCommand => new RelayCommand(() =>
        {
            dialogService.ShowDialog("AddEditHousehold");
        });


        private RelayCommand<DownloadHouseholdModel> _deleteItemCommand;

        public RelayCommand<DownloadHouseholdModel> DeleteItemCommand
        {
            get
            {
                return _deleteItemCommand ?? (_deleteItemCommand = new RelayCommand<DownloadHouseholdModel>((h) =>
                {
                    rescueClient.DeleteHousehold(h.Id, ex =>
                    {
                        if (ex == null)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                _households.Remove(h);
                            });
                            MessengerInstance.Send(new Messages.StatsChangedMessage());
                        }
                    });
                }));
            }

        }

        public RelayCommand<DownloadHouseholdModel> EditItemCommand => new RelayCommand<DownloadHouseholdModel>((h) =>
        {
            dialogService.ShowDialog<DownloadHouseholdModel>("AddEditHousehold", h);
        });

        private RelayCommand<DownloadHouseholdModel> _addMemberCommand;

        public RelayCommand<DownloadHouseholdModel> AddMemberCommand
        {
            get
            {
                return _addMemberCommand ?? (_addMemberCommand = new RelayCommand<DownloadHouseholdModel>((h) =>
                {
                    dialogService.ShowDialog("FamilyMemberSelector", h);
                }, (x) =>
                {
                    return Households.CurrentItem != null;
                }));
            }

        }


        public HouseholdsVM(RescueClient rescueClient, DialogService dialogService)
        {
            this.dialogService = dialogService;
            this.rescueClient = rescueClient;
            if (IsInDesignModeStatic)
            {
                _households.Add(new DownloadHouseholdModel
                {
                    Address = "Catbalogan City",
                    EconomicStatus = "Mayaman",
                    HouseNumber = "001",
                    IsOwned = true,
                    members = new List<DownloadPersonModel>
                    {
                        new DownloadPersonModel{
                            FirstName = "arhcie",
                            IsHead = true,
                            Contact = "1234",
                            MiddleName = "Espe",
                            LastName = "Cortez"
                        }
                    }
                });

                _households.Add(new DownloadHouseholdModel
                {
                    Address = "Catbalogan City",
                    EconomicStatus = "Mayaman",
                    HouseNumber = "001",
                    IsOwned = true
                });
            }
            else
            {
                LoadHouseholds();

                MessengerInstance.Register<Messages.AddEditResultMessage<DownloadHouseholdModel>>(this, (rslt) =>
                {
                    if (rslt.Error == null)
                    {
                        var oldHousehold = _households.FirstOrDefault(c => c.Id == rslt.Entity.Id);

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            if (oldHousehold != null)
                                _households.Remove(oldHousehold);
                            _households.Add(rslt.Entity);
                        });

                        MessengerInstance.Send(new Messages.StatsChangedMessage());
                    }
                });
            }
        }

        private void LoadHouseholds()
        {
            rescueClient.GetHouseholds((ex, hous) =>
            {
                if (ex == null)
                {
                    foreach (var item in hous)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            _households.Add(item);
                        });
                    }
                }
            });
        }

        public override void OnShow<T>(T args)
        {
            
        }
    }
}
