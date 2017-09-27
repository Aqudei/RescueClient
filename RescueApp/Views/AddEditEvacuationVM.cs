using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls.Dialogs;
using RescueApp.Models;
using RescueApp.Views.Helpers;
using RescueApp.ViewServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views
{
    public class AddEditEvacuationVM : PageBase, Interfaces.IEditorDialog<Center>
    {
        public string Title { get; set; } = "TEST";

        private string _choosenPhoto;

        public string ChoosenPhoto
        {
            get { return _choosenPhoto; }
            set { Set(ref _choosenPhoto, value); }
        }

        private string _name;

        public string CenterName
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        private string _address;

        public string Address
        {
            get { return _address; }
            set { Set(ref _address, value); }
        }

        private int _capacity;

        public int Limit
        {
            get { return _capacity; }
            set { Set(ref _capacity, value); }
        }

        private readonly RescueClient rescueClient;
        private readonly DialogService dialogService;
        private readonly IDialogCoordinator dialogCoordinator;
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

        public AddEditEvacuationVM(RescueClient rescueClient, DialogService dialogService,
            IDialogCoordinator dialogCoordinator)
        {
            this.rescueClient = rescueClient;
            this.dialogService = dialogService;
            this.dialogCoordinator = dialogCoordinator;
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }


        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(() =>
                {
                    var center = new Center
                    {
                        Address = Address,
                        CenterName = CenterName,
                        Limit = Limit,
                        Id = Id,
                        Photo = ChoosenPhoto

                    };

                    if (Id == 0)
                        CreateEvacuationCenter(center);
                    else
                        UpdateEvacuationCenter(center);
                }));
            }
        }

        private void UpdateEvacuationCenter(Center center)
        {
            rescueClient.UpdateCenter(center, (ex, updatedCenter) =>
            {
                if (ex == null)
                {
                    ClearFields();
                    dialogCoordinator.ShowMessageAsync(this, "Save Operation Success",
                        string.Format("Evacuation Center named {0} has been saved.", center.CenterName));
                }
                else
                {
                    dialogCoordinator.ShowMessageAsync(this, "Save Operation Failed",
                        string.Format("Evacuation Center not saved.\n{0}", ex.Message));
                }

                MessengerInstance.Send(new Messages.AddEditResultMessage<Center>(ex, updatedCenter));
            });
        }

        private void CreateEvacuationCenter(Center center)
        {
            rescueClient.AddCenter(center, (ex, _center) =>
            {
                if (ex == null)
                {
                    ClearFields();
                    MessengerInstance.Send(new Messages.AddEditResultMessage<Center>(ex, _center));
                    dialogCoordinator.ShowMessageAsync(this, "Save Operation Success",
                        string.Format("Evacuation Center named {0} has been saved.", _center.CenterName));
                }
                else
                {
                    dialogCoordinator.ShowMessageAsync(this, "Save Operation Failed",
                        string.Format("Evacuation Center not saved.\n{0}", ex.Message));
                }

            });
        }

        private void ClearFields()
        {
            Address = "";
            Id = 0;
            CenterName = "";
            ChoosenPhoto = null;
            Limit = 0;
        }

        public override void DoCleanup()
        {
            ClearFields();
        }

        public override void OnShow<T>(T args)
        { }

        public void Edit(Center item)
        {
            Address = item.Address;
            Id = item.Id;
            CenterName = item.CenterName;
            ChoosenPhoto = item.Photo;
            Limit = item.Limit;
        }
    }
}
