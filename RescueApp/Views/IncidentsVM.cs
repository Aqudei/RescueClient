using GalaSoft.MvvmLight;
using RescueApp.Models;
using RescueApp.Views.Helpers;
using RescueApp.ViewServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views
{
    public class IncidentsVM : PageBase
    {
        private readonly DialogService dialogService;
        private readonly RescueClient rescueClient;

        public String Title { get; set; } = "Incidents";

        public ObservableCollection<Incident> incidents { get; set; } = new ObservableCollection<Incident>();

        public IncidentsVM(DialogService dialogService, RescueClient rescueClient)
        {
            this.dialogService = dialogService;
            this.rescueClient = rescueClient;


        }

        public override void OnShow<T>(T args)
        {
            
        }
    }
}
