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

namespace RescueApp.Views
{
    public class FamilyMemberSelectorVM : ViewModelBase, IEditor<DownloadHouseholdModel>
    {
        private ObservableCollection<DownloadPersonModel> _people
            = new ObservableCollection<DownloadPersonModel>();

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


        public FamilyMemberSelectorVM(RescueClient rescueClient)
        {
            this.rescueClient = rescueClient;
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
                            _people.Add(item);
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
