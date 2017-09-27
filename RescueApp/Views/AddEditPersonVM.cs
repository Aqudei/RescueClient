using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls.Dialogs;
using RescueApp.Interfaces;
using RescueApp.Messages;
using RescueApp.Models;
using RescueApp.Views.Helpers;
using RescueApp.ViewServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views
{
    public class AddEditPersonVM : PageBase, IEditorDialog<DownloadPersonModel>
    {
        private string _choosenPhoto;
        public string ChoosenPhoto
        {
            get { return _choosenPhoto; }
            set { Set(ref _choosenPhoto, value); }
        }

        private readonly RescueClient _rescueClient;
        private readonly DialogService _dialogService;
        private readonly IDialogCoordinator dialog;

        public int Id { get; set; }
        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set { Set(ref _firstName, value); }
        }

        private string _middleName;

        public string MiddleName
        {
            get { return _middleName; }
            set { Set(ref _middleName, value); }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set { Set(ref _lastName, value); }
        }

        public DateTime? Birthday { get; set; }
        private string _bloodType;

        public string BloodType
        {
            get { return _bloodType; }
            set { Set(ref _bloodType, value); }
        }

        private string _address;

        public string Address
        {
            get { return _address; }
            set { Set(ref _address, value); }
        }


        private string _gender;
        public string Gender
        {
            get { return _gender; }
            set { Set(ref _gender, value); }
        }

        private string _namePrefix;
        public string NamePrefix
        {
            get { return _namePrefix; }
            set { Set(ref _namePrefix, value); }
        }

        private string _nameSuffix;
        public string NameSuffix
        {
            get { return _nameSuffix; }
            set { Set(ref _nameSuffix, value); }
        }


        public AddEditPersonVM(RescueClient client,
            DialogService dialogService, IDialogCoordinator dialog)
        {
            _rescueClient = client;
            _dialogService = dialogService;
            this.dialog = dialog;
            MessengerInstance.Register<Dialogs.CapturedPhotoEvenArgs>(this, (p) =>
            {
                ChoosenPhoto = p.PhotoPath;
            });
        }

        private RelayCommand _browsePhotoCommand;
        public RelayCommand BrowsePhotoCommand
        {
            get
            {
                return _browsePhotoCommand ?? (_browsePhotoCommand = new RelayCommand(() =>
                {
                    ChoosenPhoto = _dialogService.ShowOpenFileDialog();
                }));
            }
        }

        private RelayCommand _openCameraCommand;

        public RelayCommand OpenCameraCommand
        {
            get
            {
                return _openCameraCommand ?? (_openCameraCommand = new RelayCommand(() =>
                {
                    _dialogService.ShowCamera();
                }));
            }
        }


        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(() =>
                {
                    var person = AutoMapper.Mapper.Map<DownloadPersonModel>(this);
                    person.Birthday = Birthday.HasValue ? Birthday.Value.ToShortDateString() : null;

                    if (Id > 0)
                    {
                        person.Id = Id;
                        var personMinusPhoto = AutoMapper.Mapper.Map<UploadPersonModel>(person);
                        _rescueClient.UpdatePerson(personMinusPhoto, (ex, rslt) =>
                        {
                            if (ex == null)
                            {
                                ClearFields();
                                dialog.ShowMessageAsync(this, "SAVE OPERATION SUCCESSFULL",
                                    string.Format("A person with name {0} has been saved", rslt.FullName));
                            }
                            else
                            {
                                dialog.ShowMessageAsync(this, "SAVE OPERATION FAILURE",
                                    "Person not saved");
                            }

                            MessengerInstance.Send(new AddEditResultMessage<DownloadPersonModel>(ex, rslt));
                        }, ChoosenPhoto);
                    }
                    else
                    {
                        _rescueClient.AddPerson(person, (ex, p) =>
                        {
                            if (ex == null)
                            {
                                ClearFields();
                                dialog.ShowMessageAsync(this, "SAVE OPERATION SUCCESSFULL",
                                  string.Format("A person with name {0} has been saved", p.FullName));
                            }
                            else
                            {
                                dialog.ShowMessageAsync(this, "SAVE OPERATION FAILURE",
                                   string.Format("Person not saved\n{0}", ex.Message));
                            }

                            MessengerInstance.Send(new AddEditResultMessage<DownloadPersonModel>(ex, p));
                        }, ChoosenPhoto);
                    }
                }));
            }
        }

        private void ClearFields()
        {
            ChoosenPhoto = null;
            FirstName = "";
            MiddleName = "";
            LastName = "";
            Id = 0;
            Birthday = null;
            Address = "";
            Contact = "";
            Occupation = "";
            MedicalCondition = "";
            MedicineRequired = "";
            Vulnerabilities = "";
            NamePrefix = "";
            NameSuffix = "";
        }

        public override void DoCleanup()
        {
            ClearFields();
        }

        public void Edit(DownloadPersonModel item)
        {
            CivilStatus = item.CivilStatus;
            Education = item.EducationalAttainment;
            Email = item.Email;
            Gender = item.Gender;
            MedicalCondition = item.MedicalCondition;
            MedicineRequired = item.MedicineRequired;
            NationalIdNumber = item.NationalIdNumber;
            Occupation = item.Occupation;
            Vulnerabilities = item.Vulnerabilities;
            BloodType = item.BloodType;
            Allergies = item.Allergies;
            IsHead = item.IsHead;
            FirstName = item.FirstName;
            MiddleName = item.MiddleName;
            LastName = item.LastName;
            Id = item.Id;
            Contact = item.Contact;
            ChoosenPhoto = item.Photo;
            if (string.IsNullOrEmpty(item.Birthday))
                return;
            Birthday = DateTime.Parse(item.Birthday);
        }

        public override void OnShow<T>(T args)
        {

        }

        private String _contact;
        public String Contact
        {
            get { return _contact; }
            set { Set(ref _contact, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }

        private bool _isHead;
        public bool IsHead
        {
            get { return _isHead; }
            set { Set(ref _isHead, value); }
        }

        private string _nationalIdNumber;
        public string NationalIdNumber
        {
            get { return _nationalIdNumber; }
            set { Set(ref _nationalIdNumber, value); }
        }

        private string _vulnerabilities;
        public string Vulnerabilities
        {
            get { return _vulnerabilities; }
            set { Set(ref _vulnerabilities, value); }
        }

        private string _education;
        public string Education
        {
            get { return _education; }
            set { Set(ref _education, value); }
        }

        private string _allergies;
        public string Allergies
        {
            get { return _allergies; }
            set { Set(ref _allergies, value); }
        }

        private string _medicalCondition;
        public string MedicalCondition
        {
            get { return _medicalCondition; }
            set { Set(ref _medicalCondition, value); }
        }

        private string _medicineRequired;
        public string MedicineRequired
        {
            get { return _medicineRequired; }
            set { Set(ref _medicineRequired, value); }
        }

        private string _occupation;
        public string Occupation
        {
            get { return _occupation; }
            set { Set(ref _occupation, value); }
        }

        private string _civilStatus;

        public string CivilStatus
        {
            get { return _civilStatus; }
            set { Set(ref _civilStatus, value); }
        }
    }
}
