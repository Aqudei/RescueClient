/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:RescueApp"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using RescueApp.Views;
using RescueApp.ViewServices;
using RescueApp.Models;
using RescueApp.Views.Dialogs;

namespace RescueApp.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AddEditPersonVM, DownloadPersonModel>();
                cfg.CreateMap<DownloadPersonModel, AddEditPersonVM>();
                cfg.CreateMap<AddEditEvacuationVM, Center>();
                cfg.CreateMap<Center, AddEditEvacuationVM>();
                cfg.CreateMap<DownloadPersonModel, UploadPersonModel>();
            });
            SimpleIoc.Default.Register<AddEditHouseholdVM>();
            SimpleIoc.Default.Register<HouseholdsVM>();
            SimpleIoc.Default.Register<DialogService>();
            SimpleIoc.Default.Register<MissionStatementVM>();
            SimpleIoc.Default.Register<IncidentsVM>();
            SimpleIoc.Default.Register<MonitoringVM>();
            SimpleIoc.Default.Register<EvacuationListVM>();
            SimpleIoc.Default.Register<AddEditPersonVM>();
            SimpleIoc.Default.Register<RescueClient>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<PeopleVM>();
            SimpleIoc.Default.Register<AddEditEvacuationVM>();


            if (!ViewModelBase.IsInDesignModeStatic)
            {
                InitializeDialogs();
            }
        }


        public AddEditHouseholdVM AddEditHouseholdVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddEditHouseholdVM>();
            }
        }

        public HouseholdsVM HouseholdsVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HouseholdsVM>();
            }
        }

        public AddEditEvacuationVM AddEditEvacuationVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddEditEvacuationVM>();
            }
        }

        public IncidentsVM IncidentsVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IncidentsVM>();
            }
        }

        public MonitoringVM MonitoringVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MonitoringVM>();
            }
        }

        public MissionStatementVM MissionStatementVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MissionStatementVM>();
            }
        }

        public AddEditPersonVM AddEditPersonVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddEditPersonVM>();
            }
        }

        public EvacuationListVM EvacuationListVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EvacuationListVM>();
            }
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public PeopleVM PeopleVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PeopleVM>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        private void InitializeDialogs()
        {
            var dialogService = SimpleIoc.Default.GetInstance<DialogService>();
            dialogService.RegisterDialog<AddEditPerson>("AddEditPerson");
            dialogService.RegisterDialog<AddEditEvacuation>("AddEditEvacuation");
            dialogService.RegisterDialog<AddEditHousehold>("AddEditHousehold");
            dialogService.RegisterDialog<Camera>("Camera");
        }
    }
}