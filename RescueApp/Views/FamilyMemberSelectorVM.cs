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
            private bool _selected;
            public bool Selected
            {
                get
                {
                    return _selected;
                }
                set
                {
                    Set(ref _selected, value);
                }
            }
        }

        private ObservableCollection<SelectablePerson> _people
            = new ObservableCollection<SelectablePerson>();

        DownloadHouseholdModel CurrentHousehold
            = new DownloadHouseholdModel();

        private ICollectionView _peopleView;
        public ICollectionView PeopleView
        {
            get
            {
                return _peopleView ?? (_peopleView = CollectionViewSource.GetDefaultView(_people));
            }
        }

        private string filterText;
        private readonly RescueClient rescueClient;

        public string FilterText
        {
            get { return filterText; }
            set { Set(ref filterText, value); }
        }

        private RelayCommand<DownloadPersonModel> _toggleMembership;

        public RelayCommand<DownloadPersonModel> ToggleMembership
        {
            get
            {
                return _toggleMembership ?? (_toggleMembership = new RelayCommand<DownloadPersonModel>((person) =>
                {
                    rescueClient.SetMembership(person, CurrentHousehold, (ex, household) =>
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
                    Selected = true
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
                                Selected = CurrentHousehold.members.Where(m => m.Id == item.Id).Any()
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
