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

namespace RescueApp.Views
{
    public class FamilyMemberSelectorVM : ViewModelBase, IEditor<DownloadHouseholdModel>
    {
        private readonly RescueClient rescueClient;

        public DownloadHouseholdModel CurrentHousehold { get; set; }
            = new DownloadHouseholdModel();

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
            else
                LoadAllPeople();
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


        private RelayCommand<DownloadPersonModel> _toggleMembership;
        public RelayCommand<DownloadPersonModel> ToggleMembership
        {
            get
            {
                return _toggleMembership ?? (_toggleMembership = new RelayCommand<DownloadPersonModel>((p) =>
                {
                    rescueClient.ToggleMembership(p, CurrentHousehold, (ex, household_new) =>
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            Edit(household_new);
                        });

                        foreach (var item in household_new.members)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {

                            });
                        }
                    });
                }));
            }
        }

        private void LoadAllPeople()
        {
            _allPeople.Clear();
            rescueClient.GetPeople((ex, people) =>
            {
                if (ex == null)
                {
                    foreach (var item in people)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            _allPeople.Add(item);
                        });
                    }
                }
            });
        }

        public void Edit(DownloadHouseholdModel item)
        {
            AutoMapper.Mapper.Map(item, CurrentHousehold,
                typeof(DownloadHouseholdModel), typeof(DownloadHouseholdModel));
        }
    }
}
