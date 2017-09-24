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

namespace RescueApp.Views
{
    public class HouseholdsVM : ViewModelBase, ICrudVM<DownloadHouseholdModel>
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

        public RelayCommand<DownloadHouseholdModel> DeleteItemCommand => new RelayCommand<DownloadHouseholdModel>(h =>
        {
            rescueClient.DeleteHousehold(h.Id, ex =>
            {
                if (ex == null)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        _households.Remove(h);
                    });
                }
            });
        });

        public RelayCommand<DownloadHouseholdModel> EditItemCommand => new RelayCommand<DownloadHouseholdModel>((h) =>
        {
            dialogService.ShowDialog<DownloadHouseholdModel>("AddEditHousehold", h);
        });

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
        }
    }
}
