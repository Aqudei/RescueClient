﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Models
{
    public class UploadPersonModel : WithID
    {
        private String firstName;
        private String middleName;
        private String lastName;
        private string birthday;
        private String bloodType;

        public string FirstName { get => firstName; set => firstName = value; }
        public string MiddleName { get => middleName; set => middleName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Birthday { get => birthday; set => birthday = value; }
        public string BloodType { get => bloodType; set => bloodType = value; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Vulnerabilities { get; set; }
        public string NationalIdNumber { get; set; }
        public int? _Household { get; set; }
        public bool IsHead { get; set; }
        public string Gender { get; set; }
        public string EducationalAttainment { get; set; }
        public string Allergies { get; set; }
        public string MedicalCondition { get; set; }
        public string MedicineRequired { get; set; }
        public string CivilStatus { get; set; }
        public string Occupation { get; set; }
        public string NamePrefix { get; set; }
        public string NameSuffix { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1} {2}", lastName, firstName, middleName);
        }

        public string FullName
        {
            get
            {
                return ToString();
            }
        }

        public bool IsVulnerable
        {
            get
            {
                return !string.IsNullOrEmpty(Vulnerabilities);
            }
        }
        public string Age
        {
            get
            {
                if (false == string.IsNullOrEmpty(birthday))
                {
                    var date = DateTime.Parse(birthday);
                    return "" + Math.Floor(((DateTime.Now - date).TotalDays / 365));
                }

                return "";
            }
        }

    }
}
