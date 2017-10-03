using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using RescueApp.Views;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using RescueApp.Views.Helpers;
using RescueApp.Interfaces;

namespace RescueApp.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : PageBase
    {
        public string AppTitle { get; set; } = "DISASTER+RISK REDUCTION MANAGEMENT SYSTEM";

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        { }

        public List<PageBase> Screens { get; set; } = new List<PageBase>
        {
            SimpleIoc.Default.GetInstance<MissionStatementVM>(),
            SimpleIoc.Default.GetInstance<HouseholdsVM>(),
            SimpleIoc.Default.GetInstance<PeopleVM>(),
            SimpleIoc.Default.GetInstance<EvacuationListVM>(),
            SimpleIoc.Default.GetInstance<IncidentsVM>(),
            SimpleIoc.Default.GetInstance<MonitoringVM>()
        };

        private PageBase _selectedScreen;

        public PageBase SelectedScreen
        {
            get { return _selectedScreen; }
            set
            {
                _selectedScreen = value;
                var navigable = value as INavigable;
                navigable?.OnNavigated();

                RaisePropertyChanged(nameof(VMCanAddItem));
            }
        }


        public bool VMCanAddItem
        {
            get
            {
                if (SelectedScreen != null)
                {
                    return SelectedScreen.GetType().GetInterfaces().Any(x => x.IsGenericType &&
                    x.GetGenericTypeDefinition() == typeof(ICrudVM<>));
                }

                return false;
            }
        }

        //public override void OnShow<T>(T args)
        //{ }
    }
}