using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Models
{
    public class Person : HasId
    {
        String firstName;
        String middleName;
        String lastName;
        string birthday;
        String bloodType;

        public string Photo { get; set; }

        public string FirstName { get => firstName; set => firstName = value; }
        public string MiddleName { get => middleName; set => middleName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Birthday { get => birthday; set => birthday = value; }
        public string BloodType { get => bloodType; set => bloodType = value; }

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

        public string Address { get; set; }

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

        public string Sickness { get; set; }
        public string Contact { get; set; }
        public string Members { get; set; }

        public List<String> FamilyMembers
        {
            get
            {
                if (string.IsNullOrEmpty(Members))
                    return null;

                return Members.Split(";".ToCharArray()).ToList();
            }
        }

    }
}
