using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
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
                    Birthday = new DateTime(1989, 12, 22)
                });

                People.Add(new Person
                {
                    FirstName = "Kram",
                    MiddleName = "Espe",
                    LastName = "Airbytes",
                    Address = "Port Area, Tacloban City",
                    Birthday = new DateTime(1989, 12, 22)
                });
                People.Add(new Person
                {
                    FirstName = "Kram",
                    MiddleName = "Espe",
                    LastName = "Airbytes",
                    Address = "Port Area, Tacloban City",
                    Birthday = new DateTime(1989, 12, 22)
                });
                People.Add(new Person
                {
                    FirstName = "Kram",
                    MiddleName = "Espe",
                    LastName = "Airbytes",
                    Address = "Port Area, Tacloban City",
                    Birthday = new DateTime(1989, 12, 22)
                });
                People.Add(new Person
                {
                    FirstName = "Kram",
                    MiddleName = "Espe",
                    LastName = "Airbytes",
                    Address = "Port Area, Tacloban City",
                    Birthday = new DateTime(1989, 12, 22)
                });
                People.Add(new Person
                {
                    FirstName = "Kram",
                    MiddleName = "Espe",
                    LastName = "Airbytes",
                    Address = "Port Area, Tacloban City",
                    Birthday = new DateTime(1989, 12, 22)
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
