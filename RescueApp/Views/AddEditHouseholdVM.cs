using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls.Dialogs;
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
using System.Windows.Data;

namespace RescueApp.Views
{
    public class AddEditHouseholdVM : PageBase,
        IEditorDialog<DownloadHouseholdModel>
    {
        private readonly DialogService dialogService;
        private readonly IDialogCoordinator dialogCoordinator;
        private readonly RescueClient rescueClient;

        public DownloadHouseholdModel Current { get; set; } = new DownloadHouseholdModel();

        private string _choosenPhoto;

        public string ChoosenPhoto
        {
            get { return _choosenPhoto; }
            set { Set(ref _choosenPhoto, value); }
        }

        public void Edit(DownloadHouseholdModel item)
        {
            AutoMapper.Mapper.Map(item, Current, typeof(DownloadHouseholdModel), typeof(DownloadHouseholdModel));
        }

        public AddEditHouseholdVM(RescueClient rescueClient, DialogService dialogService,
            IDialogCoordinator dialogCoordinator)
        {
            this.rescueClient = rescueClient;
            this.dialogService = dialogService;
            this.dialogCoordinator = dialogCoordinator;
        }

        //public override void OnShow<T>(T args)
        //{ }

        private RelayCommand _browsePhotoCommand;
        public RelayCommand BrosePhotoCommand
        {
            get
            {
                return _browsePhotoCommand ?? (_browsePhotoCommand = new RelayCommand(() =>
                {
                    var photo = dialogService.ShowOpenFileDialog();
                    ChoosenPhoto = photo;
                }));
            }
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(() =>
                {
                    var uploadHS = AutoMapper.Mapper.Map<UploadHouseholdModel>(Current);

                    if (uploadHS.Id == 0)
                    {
                        rescueClient.AddHousehold(uploadHS, (ex, hs) =>
                        {
                            if (ex == null)
                            {
                                ClearFields();
                                dialogCoordinator.ShowMessageAsync(this, "Success", "HOUSEHOLD INFO CHANGED");
                            }
                            else
                            {
                                dialogCoordinator.ShowMessageAsync(this, "FAILURE", "FAILED TO CHANG HOUSEHOLD INFO");
                            }
                            MessengerInstance.Send(new AddEditResultMessage<DownloadHouseholdModel>(ex, hs));
                        }, ChoosenPhoto);
                    }
                    else
                    {
                        rescueClient.UpdateHousehold(uploadHS, (ex, hs) =>
                        {
                            MessengerInstance.Send(new AddEditResultMessage<DownloadHouseholdModel>(ex, hs));
                        }, ChoosenPhoto);
                    }
                }));
            }
        }

        private void ClearFields()
        {
            ChoosenPhoto = null;
            Current.Address = "";
            Current.HouseCategory = "";
            Current.HouseNumber = "";
            Current.Id = 0;
            Current.IsSafeZone = true;
            Current.IsStormSurgeProne = false;
            Current.IsTsunamiProne = false;
            Current.IsEarthquakeProne = false;
            Current.HouseCategory = "";
            Current.members?.Clear();
        }

        public override void DoCleanup()
        {
            ClearFields();
        }
    }
}
