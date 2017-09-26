using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RescueApp.Interfaces;
using RescueApp.Messages;
using RescueApp.Models;
using RescueApp.Views.Helpers;
using RescueApp.ViewServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RescueApp.Views
{
    public class AddEditHouseholdVM : PageBase,
        IEditor<DownloadHouseholdModel>
    {
        private readonly DialogService dialogService;
        private readonly RescueClient rescueClient;

        public DownloadHouseholdModel Current { get; set; } = new DownloadHouseholdModel();

        public void Edit(DownloadHouseholdModel item)
        {
            AutoMapper.Mapper.Map(item, Current, typeof(DownloadHouseholdModel), typeof(DownloadHouseholdModel));
        }

        public AddEditHouseholdVM(RescueClient rescueClient, DialogService dialogService)
        {
            this.rescueClient = rescueClient;
            this.dialogService = dialogService;

            LoadEvacuationCenters();
        }

        public ObservableCollection<Center> Centers { get; set; }
            = new ObservableCollection<Center>();

        private void LoadEvacuationCenters()
        {
            Centers.Clear();

            rescueClient.GetCenters((ex, centers) =>
            {
                if (ex == null)
                {
                    foreach (var c in centers)
                    {
                        Centers.Add(c);
                    }
                }
            });
        }

        public override void OnShow<T>(T args)
        {
            throw new NotImplementedException();
        }

        private RelayCommand _saveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(() =>
                {
                    var uploadHS = AutoMapper.Mapper.Map<UploadHouseholdModel>(Current);

                    if (uploadHS.Id == 0)
                    {
                        rescueClient.AddHousehold(uploadHS, (ex, hs) =>
                        {
                            MessengerInstance.Send(new AddEditResultMessage<DownloadHouseholdModel>(ex, hs));
                        });
                    }
                    else
                    {
                        rescueClient.UpdateHousehold(uploadHS, (ex, hs) =>
                        {
                            MessengerInstance.Send(new AddEditResultMessage<DownloadHouseholdModel>(ex, hs));
                        });
                    }
                }));
            }
        }
    }
}
