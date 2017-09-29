using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using RescueApp.Interfaces;
using RescueApp.Models;
using RescueApp.Views.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RescueApp.Views
{
    public class CenterSelectorVM : ViewModelBase, IEditorDialog<Center>
    {
        private readonly RescueClient rescueClient;

        private ICollectionView _allPeopleView;

        public ICollectionView AllPeopleView
        {
            get { return _allPeopleView ?? (_allPeopleView = CollectionViewSource.GetDefaultView(_people)); }

        }


        private Center _currentCenter;

        public Center CurrentCenter
        {
            get { return _currentCenter; }
            set { Set(ref _currentCenter, value); }
        }

        public CenterSelectorVM(RescueClient rescueClient)
        {
            this.rescueClient = rescueClient;
        }


        ObservableCollection<DownloadPersonModel> _people
            = new ObservableCollection<DownloadPersonModel>();

        IdComparer<DownloadPersonModel> comparer
            = new IdComparer<DownloadPersonModel>();

        public void Edit(Center item)
        {
            CurrentCenter = item;

            rescueClient.GetPeople((ex, ppl) =>
            {
                if (ex == null)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        _people.Clear();
                        var diff = ppl.Where(p => p._Household == null)
                            .Except(item.members, comparer);

                        foreach (var person in diff)
                        {
                            _people.Add(person);
                        }
                    });
                }
            });
        }
    }
}
