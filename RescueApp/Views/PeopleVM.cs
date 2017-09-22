using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using RescueApp.Messages;
using RescueApp.Models;
using RescueApp.ViewServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views
{
    public class PeopleVM : ViewModelBase, ICrudVM<Person>
    {
        public ObservableCollection<Person> People { get; set; }
            = new ObservableCollection<Person>();

        private readonly RescueClient _rescueClient;

        private DialogService _dialogService;

        public string Title { get; set; } = "People";

        public RelayCommand CreateItemCommand => new RelayCommand(() =>
        {
            CreateItem();
        });

        private void CreateItem()
        {
            _dialogService.ShowDialog("AddEditPerson");
        }

        public RelayCommand<Person> DeleteItemCommand => new RelayCommand<Person>((person) =>
        {
            DeleteItem(person);
        });

        private RelayCommand<Person> _editItemCommand;

        public RelayCommand<Person> EditItemCommand
        {
            get
            {
                return _editItemCommand ?? (_editItemCommand = new RelayCommand<Person>((p) =>
                {
                    _dialogService.ShowDialog("AddEditPerson", p);
                }));
            }
        }



        private void DeleteItem(Person person)
        {
            _rescueClient.DeletePerson(person.Id, (err) =>
            {
                if (err == null)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        People.Remove(person);
                    });
                }
            });
        }

        public PeopleVM(RescueClient client, DialogService dialogService)
        {
            _dialogService = dialogService;

            if (IsInDesignModeStatic)
            {
                People.Add(new Person
                {
                    FirstName = "Archie",
                    MiddleName = "Espe",
                    LastName = "Cortez",
                    Address = "Port Area, Tacloban City",
                    Birthday = "12/31/2017"
                });

                People.Add(new Person
                {
                    FirstName = "Kram",
                    MiddleName = "Espe",
                    LastName = "Airbytes",
                    Address = "Port Area, Tacloban City",
                    Birthday = "12/31/2017"
                });
                People.Add(new Person
                {
                    FirstName = "Kram",
                    MiddleName = "Espe",
                    LastName = "Airbytes",
                    Address = "Port Area, Tacloban City",
                    Birthday = "12/31/2017"
                });
                People.Add(new Person
                {
                    FirstName = "Kram",
                    MiddleName = "Espe",
                    LastName = "Airbytes",
                    Address = "Port Area, Tacloban City",
                    Birthday = "12/31/2017"
                });
                People.Add(new Person
                {
                    FirstName = "Kram",
                    MiddleName = "Espe",
                    LastName = "Airbytes",
                    Address = "Port Area, Tacloban City",
                    Birthday = "12/31/2017"
                });
                People.Add(new Person
                {
                    FirstName = "Kram",
                    MiddleName = "Espe",
                    LastName = "Airbytes",
                    Address = "Port Area, Tacloban City",
                    Birthday = "12/31/2017"
                });
            }
            else
            {
                MessengerInstance.Register<AddEditResultMessage<Person>>(this, (rslt) =>
                {
                    if (rslt.Error == null)
                    {
                        var _p = People.FirstOrDefault(p => p.Id == rslt.Entity.Id);

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            if (_p != null)
                                People.Remove(_p);
                            People.Add(rslt.Entity);
                        });
                    }
                });

                _rescueClient = client;
                client.GetPeople((err, rslt) =>
                {
                    if (err == null)
                    {
                        foreach (var item in rslt)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                People.Add(item);
                            });
                        }
                    }
                });

            }
        }
    }
}
