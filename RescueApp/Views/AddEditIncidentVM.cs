using RescueApp.Interfaces;
using RescueApp.Views.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RescueApp.Models;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls.Dialogs;

namespace RescueApp.Views
{
    public class AddEditIncidentVM : PageBase, IEditorDialog<Incident>
    {
        private Incident incident
            = new Incident();
        private readonly RescueClient rescueClient;
        private readonly IDialogCoordinator dialogCoordinator;

        public List<string> IncidentTypes => new List<string>
        {
            "Flood",
            "Typhoon",
            "Earthquake",
            "Tsunami",
            "Fire",
            "Others"
        };

        public Incident Incident
        {
            get { return incident; }
            set { Set(ref incident, value); }
        }

        public AddEditIncidentVM(RescueClient rescueClient, IDialogCoordinator dialogCoordinator)
        {
            this.rescueClient = rescueClient;
            this.dialogCoordinator = dialogCoordinator;
        }

        public void Edit(Incident item)
        {
            Incident = item;
        }

        public override void DoCleanup()
        {
            ClearFields();
        }

        private void ClearFields()
        {
            Incident.id = 0;
            Incident.IncidentName = null;
            Incident.Photo = null;
            Incident.DateOccured = null;
            Incident.DateFinished = null;
            Incident.IncidentType = null;
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(() =>
                {
                    rescueClient.AddIncident(Incident, (ex, newIncident) =>
                    {
                        if (ex == null)
                        {
                            ClearFields();
                            MessengerInstance.Send(new Messages.AddEditResultMessage<Incident>(ex, newIncident));
                            dialogCoordinator.ShowMessageAsync(this, "SAVE SUCCESSFULL", "NEW INCIDENT WAS ADDED");
                        }
                        else
                        {
                            dialogCoordinator.ShowMessageAsync(this, "SAVE ERROR", "UNABLE TO SAVE NEW INCIDENT TO DATABASE");
                        }
                    });
                }));
            }
        }
    }
}
