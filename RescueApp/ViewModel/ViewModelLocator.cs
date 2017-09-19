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

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using RescueApp.Views;
using RescueApp.ViewServices;

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

            SimpleIoc.Default.Register<DialogService>();
            SimpleIoc.Default.Register<MissionStatementVM>();
            SimpleIoc.Default.Register<IncidentsVM>();
            SimpleIoc.Default.Register<MonitoringVM>();
            SimpleIoc.Default.Register<EvacuationListVM>();
            SimpleIoc.Default.Register<AddEditPersonVM>();
            SimpleIoc.Default.Register<RescueClient>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<PeopleVM>();
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

        public MissionStatementVM MissionStatementVM {
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
    }
}