using GalaSoft.MvvmLight;
using RescueApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RescueApp.Models;
using System.ComponentModel;
using System.Windows.Data;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.CommandWpf;
using RescueApp.Views.Helpers;
using RescueApp.Messages;
using MahApps.Metro.Controls.Dialogs;

namespace RescueApp.Views
{
    public class FamilyMemberSelectorVM : PageBase, IEditorDialog<DownloadHouseholdModel>
    {
        private readonly RescueClient rescueClient;
        private readonly IDialogCoordinator dialogCoordinator;
        private DownloadHouseholdModel _currentHousehold;

        public DownloadHouseholdModel CurrentHousehold
        {
            get { return _currentHousehold; }
            set
            {
                Set(ref _currentHousehold, value);

            }
        }


        private ObservableCollection<DownloadPersonModel> _allPeople
            = new ObservableCollection<DownloadPersonModel>();

        private ICollectionView _allPeopleView;
        public ICollectionView AllPeopleView
        {
            get
            {
                _allPeopleView = _allPeopleView ?? (_allPeopleView = CollectionViewSource.GetDefaultView(_allPeople));
                _allPeopleView.CurrentChanged += (s, e) =>
                {
                    RaisePropertyChanged(() => CanAddMember);
                };

                return _allPeopleView;
            }
        }



        private ObservableCollection<DownloadPersonModel> _peopleNotMembers
            = new ObservableCollection<DownloadPersonModel>();

        private ICollectionView _peopleNotMembersView;
        public ICollectionView PeopleNotMembersView
        {
            get
            {
                _peopleNotMembersView = _peopleNotMembersView ?? (_peopleNotMembersView = CollectionViewSource.GetDefaultView(_peopleNotMembers));
                _peopleNotMembersView.CurrentChanged += (s, e) =>
                {
                    RaisePropertyChanged(() => CanRemoveMember);
                };
                return _peopleNotMembersView;
            }
        }


        public bool CanRemoveMember
        {
            get { return PeopleNotMembersView.CurrentItem != null; }

        }


        private RelayCommand<string> _applyFilterCommand;
        public RelayCommand<string> ApplyFilterCommand
        {
            get
            {
                return _applyFilterCommand ?? (_applyFilterCommand = new RelayCommand<string>((filterText) =>
                {
                    AllPeopleView.Filter = (o) =>
                    {
                        var p = (o as DownloadPersonModel);
                        if (p != null && !string.IsNullOrEmpty(filterText))
                        {
                            return p.FullName.ToLower()
                                .Contains(filterText.ToLower());
                        }
                        return true;
                    };
                }));
            }
        }

        public FamilyMemberSelectorVM(RescueClient rescueClient,
            IDialogCoordinator dialogCoordinator)
        {
            this.rescueClient = rescueClient;
            this.dialogCoordinator = dialogCoordinator;

            if (IsInDesignModeStatic)
            {
                _allPeople.Add(new

                    DownloadPersonModel
                {
                    FirstName = "DONATO",
                    MiddleName = "G",
                    LastName = "PORPORA"
                });
            }
        }

        private RelayCommand<DownloadPersonModel> _toggleMembership;
        public RelayCommand<DownloadPersonModel> ToggleMembership
        {
            get
            {
                return _toggleMembership ?? (_toggleMembership = new RelayCommand<DownloadPersonModel>((p) =>
                {
                    rescueClient.ToggleHouseholdMembership(p, CurrentHousehold, (ex, household_new) =>
                    {
                        if (ex == null)
                        {
                            MessengerInstance.Send(new AddEditResultMessage<DownloadHouseholdModel>(ex, household_new));
                            Edit(household_new);
                        }
                        else
                        {
                            dialogCoordinator.ShowMessageAsync(this, "ERROR ADDING AS FAMILY MEMBER MEMBER",
                                ex.Message);
                        }
                    });
                }));
            }
        }

        private void LoadAllPeople()
        { }

        class MyComparer : IEqualityComparer<DownloadPersonModel>
        {
            public bool Equals(DownloadPersonModel x, DownloadPersonModel y)
            {
                if (x == null || y == null)
                    return false;

                return x.Id == y.Id;
            }

            public int GetHashCode(DownloadPersonModel obj)
            {
                return obj.Id.GetHashCode();
            }
        }

        MyComparer _myComparer = new MyComparer();

        public void Edit(DownloadHouseholdModel item)
        {
            CurrentHousehold = item;

            rescueClient.GetPeople((ex, people) =>
            {
                if (ex == null)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        _allPeople.Clear();
                        var diff = people.Where(p => p._Household == null)
                        .Except(item.members, _myComparer);
                        foreach (var person in diff)
                        {
                            _allPeople.Add(person);
                        }
                    });
                }
            });
        }

        public bool CanAddMember
        {
            get { return AllPeopleView.CurrentItem != null; }
        }


        //public override void OnShow<T>(T args)
        //{ }
    }
}
