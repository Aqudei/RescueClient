using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RescueApp.Messages;
using RescueApp.Models;
using RescueApp.ViewServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views
{
    public class AddEditPersonVM : ViewModelBase
    {
        private string _choosenPhoto;

        public string ChoosenPhoto
        {
            get { return _choosenPhoto; }
            set { Set(ref _choosenPhoto, value); }
        }

        private readonly RescueClient _rescueClient;
        private readonly DialogService _dialogService;

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string BloodType { get; set; }
        public string Address { get; set; }


        public AddEditPersonVM(RescueClient client,
            DialogService dialogService)
        {
            _rescueClient = client;
            _dialogService = dialogService;
        }

        private RelayCommand _browsePhotoCommand;
        public RelayCommand BrowsePhotoCommand
        {
            get
            {
                return _browsePhotoCommand ?? (_browsePhotoCommand = new RelayCommand(() =>
                {
                    ChoosenPhoto = _dialogService.ShowOpenFileDialog();
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
                    var person = AutoMapper.Mapper.Map<Person>(this);
                    person.Birthday = Birthday.HasValue ? Birthday.Value.ToShortDateString() : null;
                    
                    if (Id > 0)
                    {
                        person.Id = Id;
                    }
                    else
                    {
                        _rescueClient.AddPerson(person, (err, p) =>
                        {
                            if (err == null)
                            {
                                MessengerInstance.Send(new AddEditResultMessage<Person>(err, p));
                            }
                        }, ChoosenPhoto);
                    }
                }));
            }
        }

        private String _contact;
        public String Contact
        {
            get { return _contact; }
            set { Set(ref _contact, value); }
        }

        private String _sickness;
        public String Sickness
        {
            get { return _sickness; }
            set { Set(ref _sickness, value); }
        }

        private String _members;
        public String Members
        {
            get { return _members; }
            set { Set(ref _members, value); }
        }
    }
}
