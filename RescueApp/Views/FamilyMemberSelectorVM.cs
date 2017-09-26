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
        public class SelectablePerson : ObservableObject
        {
            public DownloadPersonModel Person { get; set; }

            private bool _isFamilyMember;
            public bool IsFamilyMember
            {
                get
                {
                    return _isFamilyMember;
                }
                set
                {
                    Set(ref _isFamilyMember, value);
                }
            }
        }

        private ObservableCollection<SelectablePerson> _people
            = new ObservableCollection<SelectablePerson>();

        public DownloadHouseholdModel CurrentHousehold { get; set; }
            = new DownloadHouseholdModel();

        private ICollectionView _peopleView;
        public ICollectionView PeopleView
        {
            get
            {
                return _peopleView ?? (_peopleView = CollectionViewSource.GetDefaultView(_people));
            }
        }

        private readonly RescueClient rescueClient;

        private RelayCommand<string> _applyFilterCommand;

        public RelayCommand<string> ApplyFilterCommand
        {
            get
            {
                return _applyFilterCommand ?? (_applyFilterCommand = new RelayCommand<string>((filterText) =>
                {
                    PeopleView.Filter = (o) =>
                    {
                        var p = (o as SelectablePerson);
                        if (p != null && !string.IsNullOrEmpty(filterText))
                        {
                            return p.Person
                                .FullName
                                .ToLower()
                                .Contains(filterText.ToLower());
                        }
                        return true;
                    };
                }));
            }
        }

        private RelayCommand<DownloadPersonModel> _toggleMembership;

        public RelayCommand<DownloadPersonModel> ToggleMembership
        {
            get
            {
                return _toggleMembership ?? (_toggleMembership = new RelayCommand<DownloadPersonModel>((person) =>
                {
                    rescueClient.ToggleMembership(person, CurrentHousehold, (ex, household) =>
                    {
                        if (ex == null)
                        {
                            AutoMapper.Mapper.Map(household, CurrentHousehold,
                                typeof(DownloadHouseholdModel), typeof(DownloadHouseholdModel));
                        }
                    });
                }));
            }

        }


        public FamilyMemberSelectorVM(RescueClient rescueClient)
        {
            this.rescueClient = rescueClient;



            if (IsInDesignModeStatic)
            {
                _people.Add(new SelectablePerson
                {
                    Person = new DownloadPersonModel
                    {
                        FirstName = "DONATO",
                        MiddleName = "DONATO",
                        LastName = "DONATO"
                    },
                    IsFamilyMember = true
                });
            }
            else
                LoadPeople();
        }

        private void LoadPeople()
        {
            _people.Clear();
            rescueClient.GetPeople((ex, people) =>
            {
                if (ex == null)
                {
                    foreach (var item in people)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            _people.Add(new SelectablePerson
                            {
                                Person = item,
                                IsFamilyMember = CurrentHousehold.members.Where(m => m.Id == item.Id).Any()
                            });
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
