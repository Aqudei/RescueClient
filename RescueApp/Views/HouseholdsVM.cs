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
using MahApps.Metro.Controls.Dialogs;

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
        private readonly IDialogCoordinator dialogCoordinator;

        public ICollectionView Households
        {
            get
            {
                _householdsView = _householdsView ?? (_householdsView = CollectionViewSource.GetDefaultView(_households));
                _householdsView.SortDescriptions.Add(new SortDescription
                {
                    PropertyName = "HouseNumber",
                    Direction = ListSortDirection.Ascending
                });

                return _householdsView;
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
                return _deleteItemCommand ?? (_deleteItemCommand = new RelayCommand<DownloadHouseholdModel>(async (h) =>
                {

                    var rslt = await dialogCoordinator.ShowMessageAsync(this, "PLEASE CONFIRM DELETE", "DO YOU WANT TO DELETE "
                        + h.HouseNumber + " ?", MessageDialogStyle.AffirmativeAndNegative);

                    if (rslt == MessageDialogResult.Affirmative)

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
                }));
            }

        }


        public HouseholdsVM(RescueClient rescueClient, DialogService dialogService,
            IDialogCoordinator dialogCoordinator)
        {
            this.dialogService = dialogService;
            this.dialogCoordinator = dialogCoordinator;
            this.rescueClient = rescueClient;
            if (IsInDesignModeStatic)
            {
                _households.Add(new DownloadHouseholdModel
                {
                    Address = "Catbalogan City",
                    HouseCategory = "Mayaman",
                    HouseNumber = "001",
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
                    HouseCategory = "Mayaman",
                    HouseNumber = "001",
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
