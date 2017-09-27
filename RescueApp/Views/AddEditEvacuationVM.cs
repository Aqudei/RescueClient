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
    public class AddEditEvacuationVM : PageBase
    {
        public string Title { get; set; } = "TEST";

        private string _choosenPhoto;

        public string ChoosenPhoto
        {
            get { return _choosenPhoto; }
            set { Set(ref _choosenPhoto, value); }
        }

        private string _name;

        public string Name
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

        public int Capacity
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
                    rescueClient.AddCenter(new Center
                    {
                        Address = Address,
                        CenterName = Name,
                        Limit = Capacity,
                        Id = Id,
                        Photo = ChoosenPhoto

                    }, (ex, center) =>
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


                        MessengerInstance.Send(new Messages.AddEditResultMessage<Center>(ex, center));
                    });
                }));
            }
        }

        private void ClearFields()
        {
            Address = "";
            Id = 0;
            Name = "";
            ChoosenPhoto = null;
            Capacity = 0;
        }

        public override void OnShow<T>(T args)
        {

        }
    }
}
