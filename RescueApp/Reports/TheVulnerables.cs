using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Reports
{
    public class TheVulnerables
    {
        public string NameSuffix { get; set; }
        public string fullname { get; set; }
        public string Birthday { get; set; }
        public string Gender { get; set; }
        public string _Household { get; set; }
        public string Vulnerabilities { get; set; }

        public int Age
        {
            get
            {
                if (String.IsNullOrEmpty(Birthday))
                    return 0;

                return (int)((DateTime.Now - DateTime.Parse(Birthday)).TotalDays / 365);
            }
        }

        /*
          "NameSuffix": null,
        "FirstName": "Mark Lister",
        "MiddleName": "Espelimbergo",
        "LastName": "Abria",
        "Birthday": null,
        "_Household": "dksl",
        "Gender": "MALE"
         */
    }
}
