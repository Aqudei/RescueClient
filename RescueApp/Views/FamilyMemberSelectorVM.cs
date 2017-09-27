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

namespace RescueApp.Views
{
    public class FamilyMemberSelectorVM : PageBase, IEditorDialog<DownloadHouseholdModel>
    {
        private readonly RescueClient rescueClient;

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
                return _allPeopleView ?? (_allPeopleView = CollectionViewSource.GetDefaultView(_allPeople));
            }
        }

        private ObservableCollection<DownloadPersonModel> _peopleNotMembers
            = new ObservableCollection<DownloadPersonModel>();

        private ICollectionView _peopleNotMembersView;
        public ICollectionView PeopleNotMembersView
        {
            get
            {
                return _peopleNotMembersView ?? (_peopleNotMembersView = CollectionViewSource.GetDefaultView(_peopleNotMembers));
            }
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

        public FamilyMemberSelectorVM(RescueClient rescueClient)
        {
            this.rescueClient = rescueClient;

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
                    rescueClient.ToggleMembership(p, CurrentHousehold, (ex, household_new) =>
                    {
                        if (ex == null)
                        {
                            MessengerInstance.Send(new AddEditResultMessage<DownloadHouseholdModel>(ex, household_new));
                            Edit(household_new);
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
                        var diff = people.Except(item.members, _myComparer);
                        foreach (var person in diff)
                        {
                            _allPeople.Add(person);
                        }
                    });
                }
            });
        }

        public override void OnShow<T>(T args)
        {
            
        }
    }
}
