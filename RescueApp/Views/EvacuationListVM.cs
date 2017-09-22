using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Threading;
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
    public class EvacuationListVM : ViewModelBase, ICrudVM<Center>
    {
        public string Title { get; set; } = "Evacuation Centers";

        public ObservableCollection<Center> Centers { get; set; }
            = new ObservableCollection<Center>();

        public EvacuationListVM(RescueClient client, DialogService dialogService)
        {
            _rescueClient = client;
            this.dialogService = dialogService;
            if (IsInDesignModeStatic)
            {
                Centers.Add(new Center
                {
                    CenterName = "PASALUBONG CENTER",
                    Address = "Sitio Merkado, Catarman, Manila",
                    Limit = 300
                });

                Centers.Add(new Center
                {
                    CenterName = "PASALUBONG CENTER",
                    Address = "Sitio Merkado, Catarman, Manila",
                    Limit = 300
                });

                Centers.Add(new Center
                {
                    CenterName = "PASALUBONG CENTER",
                    Address = "Sitio Merkado, Catarman, Manila",
                    Limit = 300
                });

                Centers.Add(new Center
                {
                    CenterName = "PASALUBONG CENTER",
                    Address = "Sitio Merkado, Catarman, Manila",
                    Limit = 300
                });
            }
            else
            {
                _rescueClient.GetCenters((ex, rslt) =>
                {
                    if (ex == null)
                    {
                        foreach (var item in rslt)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                Centers.Add(item);
                            });
                        }
                    }
                });

                MessengerInstance.Register<Messages.AddEditResultMessage<Center>>(this, (rslt) =>
                {
                    if (rslt.Error == null)
                    {
                        var oldCenter = Centers.FirstOrDefault(c => c.Id == rslt.Entity.Id);

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            if (oldCenter != null)
                                Centers.Remove(oldCenter);
                            Centers.Add(rslt.Entity);
                        });
                    }
                });
            }
        }

        
        private readonly RescueClient _rescueClient;
        private readonly DialogService dialogService;
        private RelayCommand<Center> _deleteItemCommand;
        public RelayCommand<Center> DeleteItemCommand
        {
            get
            {
                return _deleteItemCommand ?? (_deleteItemCommand = new RelayCommand<Center>((item) =>
                {
                    _rescueClient.DeleteCenter(item.Id, ex =>
                    {
                        if (ex == null)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                Centers.Remove(item);
                            });
                        }
                    });
                }));
            }
        }

        public RelayCommand CreateItemCommand => new RelayCommand(() =>
        {
            dialogService.ShowDialog("AddEditEvacuation");
        });

        public RelayCommand<Center> EditItemCommand => throw new NotImplementedException();
    }
}
