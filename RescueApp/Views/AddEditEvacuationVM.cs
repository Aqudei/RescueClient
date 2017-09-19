using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RescueApp.Models;
using RescueApp.ViewServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views
{
    public class AddEditEvacuationVM : ViewModelBase
    {
        private string _choosenPhoto;

        public string ChoosenPhoto
        {
            get { return _choosenPhoto; }
            set { Set(ref _choosenPhoto, value); }
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }


        private readonly RescueClient rescueClient;
        private readonly DialogService dialogService;

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

        public AddEditEvacuationVM(RescueClient rescueClient, DialogService dialogService)
        {
            this.rescueClient = rescueClient;
            this.dialogService = dialogService;
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
                    rescueClient.AddCenter(new Models.Center
                    {
                        Address = Address,
                        CenterName = Name,
                        Limit = Capacity,
                        Id = Id
                    }, (ex, center) =>
                    {

                        MessengerInstance.Send(new Messages.AddEditResultMessage<Center>(ex, center));

                    }, ChoosenPhoto);
                }));
            }
        }


    }
}
