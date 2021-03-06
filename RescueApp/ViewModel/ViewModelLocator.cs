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
using MahApps.Metro.Controls.Dialogs;
using RescueApp.Misc;
using System.Diagnostics;
using GalaSoft.MvvmLight.Messaging;

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
                cfg.CreateMap<DownloadPersonModel, DownloadPersonModel>();
                cfg.CreateMap<DownloadHouseholdModel, UploadHouseholdModel>();
                cfg.CreateMap<DownloadHouseholdModel, DownloadHouseholdModel>();
                cfg.CreateMap<Incident, Incident>();
            });

            SimpleIoc.Default.Register<IDialogCoordinator, DialogCoordinator>();
            SimpleIoc.Default.Register<AddEditHouseholdVM>();
            SimpleIoc.Default.Register<AddEditIncidentVM>();
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
            SimpleIoc.Default.Register<StatisticsVM>();
            SimpleIoc.Default.Register<FamilyMemberSelectorVM>();
            SimpleIoc.Default.Register<CenterSelectorVM>();
            SimpleIoc.Default.Register<ReportingVM>();
            SimpleIoc.Default.Register<SettingsVM>();
            SimpleIoc.Default.Register<TollsVM>();

            if (!ViewModelBase.IsInDesignModeStatic)
            {
                InitializeDialogs();
                SimpleIoc.Default.Register(() => new SMSListener(
                    Properties.Settings.Default.SMS_PORT,
                    Properties.Settings.Default.SMS_BAUD
                    ), true);

            }
        }

        public TollsVM TollsVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TollsVM>();
            }
        }


        public SettingsVM SettingsVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsVM>();
            }
        }

        public ReportingVM ReportingVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ReportingVM>();
            }
        }


        public CenterSelectorVM CenterSelectorVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CenterSelectorVM>();
            }
        }



        public AddEditIncidentVM AddEditIncidentVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddEditIncidentVM>();
            }
        }

        public FamilyMemberSelectorVM FamilyMemberSelectorVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<FamilyMemberSelectorVM>();
            }
        }

        public StatisticsVM StatisticsVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<StatisticsVM>();
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
            Debug.WriteLine("CLEAN UP");
            SimpleIoc.Default.GetInstance<MonitoringVM>().Cleanup();
        }

        private void InitializeDialogs()
        {
            var dialogService = SimpleIoc.Default.GetInstance<DialogService>();
            dialogService.RegisterDialog<AddEditPerson>("AddEditPerson");
            dialogService.RegisterDialog<AddEditEvacuation>("AddEditEvacuation");
            dialogService.RegisterDialog<AddEditHousehold>("AddEditHousehold");
            dialogService.RegisterDialog<FamilyMemberSelector>("FamilyMemberSelector");
            dialogService.RegisterDialog<CenterSelector>("CenterSelector");
            dialogService.RegisterDialog<AddEditIncident>("AddEditIncident");

        }
    }
}