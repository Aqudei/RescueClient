﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls.Dialogs;
using RescueApp.Models;
using RescueApp.Views.Helpers;
using RescueApp.ViewServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views
{
    public class AddEditEvacuationVM : PageBase, Interfaces.IEditorDialog<Center>
    {
        public string Title { get; set; } = "TEST";

        private string _choosenPhoto;

        public string ChoosenPhoto
        {
            get { return _choosenPhoto; }
            set { Set(ref _choosenPhoto, value); }
        }

        private string _name;

        public string CenterName
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        private string _address;

        public string Address
        {
            get { return _address; }
            set { Set(ref _address, value); }
        }

        private int _capacity;

        public int Limit
        {
            get { return _capacity; }
            set { Set(ref _capacity, value); }
        }

        private double _latitude = 0;

        public double Latitude
        {
            get { return _latitude; }
            set { Set(ref _latitude, value); }
        }

        private double _longitude = 0;

        public double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                Set(ref _longitude, value);
            }
        }

        private readonly RescueClient rescueClient;
        private readonly DialogService dialogService;
        private readonly IDialogCoordinator dialogCoordinator;
        private RelayCommand _browsePhotoCommand;
        public RelayCommand BrosePhotoCommand
        {
            get
            {
                return _browsePhotoCommand ?? (_browsePhotoCommand = new RelayCommand(() =>
                {
                    var photo = dialogService.ShowOpenFileDialog();
                    ChoosenPhoto = photo;
                }));
            }
        }

        public AddEditEvacuationVM(RescueClient rescueClient, DialogService dialogService,
            IDialogCoordinator dialogCoordinator)
        {
            this.rescueClient = rescueClient;
            this.dialogService = dialogService;
            this.dialogCoordinator = dialogCoordinator;
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }


        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(() =>
                {
                    //var center = new Center
                    //{
                    //    Address = Address,
                    //    CenterName = CenterName,
                    //    Limit = Limit,
                    //    Id = Id,
                    //    Photo = ChoosenPhoto,
                    //    Latitude = Latitude,
                    //    Longitude = Longitude
                    //};

                    var center = AutoMapper.Mapper.Map<Center>(this);
                    center.Photo = ChoosenPhoto;
                    if (Id == 0)
                        CreateEvacuationCenter(center);
                    else
                        UpdateEvacuationCenter(center);
                }));
            }
        }

        private void UpdateEvacuationCenter(Center center)
        {
            rescueClient.UpdateCenter(center, (ex, updatedCenter) =>
            {
                if (ex == null)
                {
                    ClearFields();
                    MessengerInstance.Send(new Messages.AddEditResultMessage<Center>(null, updatedCenter));
                    dialogCoordinator.ShowMessageAsync(this, "Save Operation Success",
                        string.Format("Evacuation Center named {0} has been saved.", center.CenterName));
                }
                else
                {
                    dialogCoordinator.ShowMessageAsync(this, "Save Operation Failed",
                        string.Format("Evacuation Center not saved.\n{0}", ex.Message));
                }
            });
        }

        private void CreateEvacuationCenter(Center center)
        {
            rescueClient.AddCenter(center, (ex, _center) =>
            {
                if (ex == null)
                {
                    ClearFields();
                    MessengerInstance.Send(new Messages.AddEditResultMessage<Center>(ex, _center));
                    dialogCoordinator.ShowMessageAsync(this, "Save Operation Success",
                        string.Format("Evacuation Center named {0} has been saved.", _center.CenterName));
                }
                else
                {
                    dialogCoordinator.ShowMessageAsync(this, "Save Operation Failed",
                        string.Format("Evacuation Center not saved.\n{0}", ex.Message));
                }

            });
        }

        private RelayCommand _PickLocationCommand;

        public RelayCommand PickLocationCommand
        {
            get
            {
                return _PickLocationCommand ?? (_PickLocationCommand = new RelayCommand(() =>
                {
                    var loc = dialogService.ShowMapPicker();
                    Latitude = loc.Item1;
                    Longitude = loc.Item2;
                }));
            }
        }


        private void ClearFields()
        {
            Address = "";
            Id = 0;
            CenterName = "";
            ChoosenPhoto = null;
            Limit = 0;
            Latitude = 0;
            Longitude = 0;
            Amenities = "";
            InCharge = "";
            InChargeCellphone = "";
        }

        public override void DoCleanup()
        {
            ClearFields();
        }

        //public override void OnShow<T>(T args)
        //{ }

        public void Edit(Center item)
        {
            Address = item.Address;
            Id = item.id;
            CenterName = item.CenterName;
            ChoosenPhoto = item.Photo;
            Limit = item.Limit;
            Latitude = item.Latitude;
            Longitude = item.Latitude;
            Amenities = item.Amenities;
            InCharge = item.InCharge;
            InChargeCellphone = item.InChargeCellphone;
        }

        private string _inCharge;

        public string InCharge
        {
            get { return _inCharge; }
            set { Set(ref _inCharge, value); }
        }

        private string _inChargeCellphone;

        public string InChargeCellphone
        {
            get { return _inChargeCellphone; }
            set { Set(ref _inChargeCellphone, value); }
        }


        private string _amenities;

        public string Amenities
        {
            get { return _amenities; }
            set { Set(ref _amenities, value); }
        }


    }
}
