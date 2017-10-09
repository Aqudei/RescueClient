using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Reports
{
    public class PeopleReportRow
    {
        public class PersonInfo
        {
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public DateTime? Birthday { get; set; }
            public string _Household { get; set; }

            public string FullName
            {
                get
                {
                    return string.Format("{0},{1} {2}", LastName, FirstName, MiddleName);
                }
            }
        }

        public int id { get; set; }
        public PersonInfo Person { get; set; }
        public string HouseholdNumber
        {
            get
            {
                return Person?._Household;
            }
        }

        public string Name
        {
            get
            {
                return Person?.FullName;
            }
        }

        public string Age
        {
            get
            {
                if (Person.Birthday.HasValue)
                {
                    return Math.Floor(((DateTime.Now - Person.Birthday.Value).TotalDays / 365)).ToString();
                }

                return "-";
            }
        }
        public string Status { get; set; }
    }
}
