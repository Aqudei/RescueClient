using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using RescueApp.Messages;
using RescueApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views
{
    public class PeopleVM : ViewModelBase
    {
        public ObservableCollection<Person> People { get; set; }
            = new ObservableCollection<Person>();

        private readonly RescueClient _rescueClient;

        private RelayCommand _registerCommand;
        public RelayCommand RegisterCommand
        {
            get
            {
                return _registerCommand ?? (_registerCommand = new RelayCommand(() =>
                {
                    //MessengerInstance.Send(new Person());
                    Person p = new Person();
                    Messenger.Default.Send(new AddEditMessage<Person>(p));
                }));
            }
        }

        private RelayCommand<Person> _deleteCommand;
        public RelayCommand<Person> DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = new RelayCommand<Person>((p) =>
                {
                    _rescueClient.DeletePerson(p.Id, (err) =>
                    {
                        if (err == null)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                People.Remove(p);
                            });
                        }
                    });
                }));
            }
        }


        public string Title { get; set; } = "People";

        public PeopleVM(RescueClient client)
        {
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

                        GalaSoft.MvvmLight.Threading.DispatcherHelper.CheckBeginInvokeOnUI(() =>
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
                           GalaSoft.MvvmLight.Threading.DispatcherHelper.CheckBeginInvokeOnUI(() =>
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
