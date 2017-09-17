using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RescueApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views
{
    public class AddEditPersonVM : ViewModelBase
    {
        private RescueClient _rescueClient;

        public Person Person { get; set; }

        public AddEditPersonVM(RescueClient client)
        {
            _rescueClient = client;
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(() =>
                {
                    if (Person.Id <= 0)
                    {
                        //New
                        _rescueClient.AddPerson(Person, (err, newP) =>
                        {
                            if (err == null)
                            {

                            }
                        });
                    }
                }));
            }
        }

    }
}
