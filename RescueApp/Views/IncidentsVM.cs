﻿using GalaSoft.MvvmLight;
using RescueApp.Models;
using RescueApp.Views.Helpers;
using RescueApp.ViewServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
using System.Windows.Data;
using System.ComponentModel;
using MahApps.Metro.Controls.Dialogs;

namespace RescueApp.Views
{
    public class IncidentsVM : PageBase, ICrudVM<Incident>
    {
        private readonly DialogService dialogService;
        private readonly RescueClient rescueClient;
        private readonly IDialogCoordinator dialogCoordinator;

        public String Title { get; set; } = "Calamities";

        public ObservableCollection<Incident> Incidents { get; set; } = new ObservableCollection<Incident>();

        private ICollectionView _incidentsCollectionView;
        public ICollectionView IncidentsCollectionView
        {
            get
            {
                if (_incidentsCollectionView == null)
                {
                    _incidentsCollectionView = CollectionViewSource.GetDefaultView(Incidents);
                    _incidentsCollectionView.SortDescriptions.Add(new SortDescription("IncidentName", ListSortDirection.Ascending));
                }

                return _incidentsCollectionView;
            }
        }

        public RelayCommand CreateItemCommand => new RelayCommand(() => dialogService.ShowDialog("AddEditIncident"));

        public RelayCommand<Incident> DeleteItemCommand => new RelayCommand<Incident>(async (i) =>
        {
            var answer = await dialogCoordinator.ShowMessageAsync(this, "CONFIRM DELETE",
                "Are you sure you want to delete " + i.IncidentName + "?",
                MessageDialogStyle.AffirmativeAndNegative);

            if (answer == MessageDialogResult.Affirmative)
            {
                rescueClient.DeleteIncident(i, (ex) =>
                {
                    if (ex == null)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() => Incidents.Remove(i));
                    }
                });
            }
        });

        public RelayCommand<Incident> EditItemCommand => new RelayCommand<Incident>((i) =>
        {
            var _incident = AutoMapper.Mapper.Map<Incident>(i);
            dialogService.ShowDialog("AddEditIncident", _incident);
        });

        public IncidentsVM(DialogService dialogService, RescueClient rescueClient, IDialogCoordinator dialogCoordinator)
        {
            this.dialogService = dialogService;
            this.rescueClient = rescueClient;
            this.dialogCoordinator = dialogCoordinator;

            LoadIncidents();

            MessengerInstance.Register<Messages.AddEditResultMessage<Incident>>(this, rslt =>
            {
                if (rslt.Error == null)
                {
                    var existingIncident = Incidents.Where(i => i.id == rslt.Entity.id)
                        .FirstOrDefault();
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        if (existingIncident != null)
                            Incidents.Remove(existingIncident);
                        Incidents.Add(rslt.Entity);
                    });
                }
            });
        }

        public Incident SelectedIncident { get; set; }

        private void LoadIncidents()
        {
            Incidents.Clear();
            rescueClient.GetIncidents((ex, _incidents) =>
            {
                if (ex == null)
                {
                    foreach (var i in _incidents)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            Incidents.Add(i);
                        });
                    }
                }
            });
        }

        //public override void OnShow<T>(T args)
        //{ }

        private RelayCommand<Incident> _toggleAcceptCheckIn;

        public RelayCommand<Incident> ToggleAcceptCheckIn
        {
            get
            {
                return _toggleAcceptCheckIn ?? (_toggleAcceptCheckIn = new RelayCommand<Incident>((i) =>
                {
                    rescueClient.ToggleIncidentStatus(i, (ex, rslt) =>
                    {
                        if (ex == null)
                        {
                            foreach (var item in rslt)
                            {
                                var existing = Incidents.Where(_i => _i.id == item.id).FirstOrDefault();
                                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                {
                                    if (existing != null)
                                    {
                                        Incidents.Remove(existing);
                                    }

                                    Incidents.Add(item);
                                });
                            }
                        }
                    });
                }));
            }

        }



    }
}
