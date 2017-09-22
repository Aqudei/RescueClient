using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RescueApp.Interfaces;
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
    public class AddEditPersonVM : ViewModelBase, IEditor<Person>
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
        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set { Set(ref _firstName, value); }
        }

        private string _middleName;

        public string MiddleName
        {
            get { return _middleName; }
            set { Set(ref _middleName, value); }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set { Set(ref _lastName, value); }
        }

        public DateTime? Birthday { get; set; }
        private string _bloodType;

        public string BloodType
        {
            get { return _bloodType; }
            set { Set(ref _bloodType, value); }
        }

        private string _address;

        public string Address
        {
            get { return _address; }
            set { Set(ref _address, value); }
        }

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
                        _rescueClient.AddPerson(person, (ex, p) =>
                        {
                            if (ex == null)
                            {
                                ClearFields();
                            }

                            MessengerInstance.Send(new AddEditResultMessage<Person>(ex, p));
                        }, ChoosenPhoto);
                    }
                }));
            }
        }

        private void ClearFields()
        {
            ChoosenPhoto = null;
            FirstName = "";
            MiddleName = "";
            LastName = "";
            Id = 0;
            Birthday = DateTime.Now;
            Address = "";
            Sickness = "";
            Contact = "";
        }

        public void Edit(Person item)
        {
            FirstName = item.FirstName;
            MiddleName = item.MiddleName;
            LastName = item.LastName;
            Id = item.Id;
            Sickness = item.Sickness;
            Contact = item.Contact;
            Address = item.Address;
            ChoosenPhoto = item.Photo;
            if (string.IsNullOrEmpty(item.Birthday))
                return;
            Birthday = DateTime.Parse(item.Birthday);
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
