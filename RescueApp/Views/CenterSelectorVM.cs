using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Threading;
using MahApps.Metro.Controls.Dialogs;
using RescueApp.Interfaces;
using RescueApp.Messages;
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
        private readonly IDialogCoordinator dialogCoordinator;
        private ICollectionView _allPeopleView;

        public ICollectionView AllPeopleView
        {
            get { return _allPeopleView ?? (_allPeopleView = CollectionViewSource.GetDefaultView(_people)); }

        }




        public bool CanAddPerson
        {

            get { return AllPeopleView.CurrentItem != null; }
        }

        public bool CanRemovePerson
        {
            get { return CurrentSelected != null; }
        }

        public Center CurrentSelected { get; set; }


        private Center _currentCenter;

        public Center CurrentCenter
        {
            get { return _currentCenter; }
            set { Set(ref _currentCenter, value); }
        }

        public RelayCommand<string> ApplyFilterCommand => new RelayCommand<string>(filter =>
        {
            AllPeopleView.Filter = new Predicate<object>((p) =>
            {
                var person = p as DownloadPersonModel;
                if (person == null)
                    return true;

                if (string.IsNullOrEmpty(filter))
                    return true;

                if (person.FullName.ToLower().Contains(filter.ToLower()))
                    return true;

                return false;
            });
        });

        public CenterSelectorVM(RescueClient rescueClient, IDialogCoordinator dialogCoordinator)
        {
            this.rescueClient = rescueClient;
            this.dialogCoordinator = dialogCoordinator;
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
                        var diff = ppl.Where(p => p._Center == null)
                            .Except(item.members, comparer);

                        foreach (var person in diff)
                        {
                            _people.Add(person);
                        }
                    });
                }
            });
        }

        public RelayCommand<DownloadPersonModel> ToggleMembership => new RelayCommand<DownloadPersonModel>((p) =>
        {
            rescueClient.ToggleEvacuationMembership(p, CurrentCenter, (ex, center_new) =>
            {
                if (ex == null)
                {
                    MessengerInstance.Send(new AddEditResultMessage<Center>(ex, center_new));
                    Edit(center_new);
                }
                else
                {
                    dialogCoordinator.ShowMessageAsync(this, "ERROR ADDING AS FAMILY MEMBER MEMBER",
                        ex.Message);
                }
            });
        });
    }
}
