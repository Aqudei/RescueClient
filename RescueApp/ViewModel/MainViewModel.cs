using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using RescueApp.Views;
using System.Collections.Generic;
using System.Diagnostics;

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
    public class MainViewModel : ViewModelBase
    {
        private readonly RescueClient _rescueClient;

        public string AppTitle { get; set; } = "DISASTER+RISK REDUCTION MANAGEMENT SYSTEM";

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(RescueClient rescueClient)
        { }

        private ViewModelBase _currentScreen;

        private ViewModelBase _previouseScreen;

        public ViewModelBase CurrentScreen
        {
            get { return _currentScreen; }
            set
            {
                _previouseScreen = _currentScreen;

                Set(ref _currentScreen, value);
                RaisePropertyChanged(() => ShowMissionStatement);
            }
        }

        private RelayCommand _toPeopleCommand;

        public RelayCommand ToPeopleCommand
        {
            get
            {
                return _toPeopleCommand ?? (_toPeopleCommand = new RelayCommand(() =>
                {
                    CurrentScreen = SimpleIoc.Default.GetInstance<PeopleVM>();
                }));
            }
        }

        public bool ShowMissionStatement
        {
            get { return CurrentScreen == null; }
        }
    }
}