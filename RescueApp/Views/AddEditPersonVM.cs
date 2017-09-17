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
        private readonly RescueClient _rescueClient;
        private readonly DialogService _dialogService;

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string BloodType { get; set; }
        public string Address { get; set; }

        public AddEditPersonVM(RescueClient client, DialogService dialogService)
        {
            _rescueClient = client;
            _dialogService = dialogService;
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(() =>
                {
                    var person = new Person
                    {
                        Address = Address,
                        Birthday = Birthday,
                        BloodType = BloodType,
                        FirstName = FirstName,
                        LastName = LastName,
                        MiddleName = MiddleName
                    };

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
                        });
                    }

                }));
            }
        }

    }
}
