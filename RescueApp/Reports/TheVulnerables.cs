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
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public string _Household { get; set; }

        public int Age
        {
            get
            {
                if (Birthday.HasValue)
                {
                    return (int)((DateTime.Now - Birthday.Value).TotalDays / 365);
                }

                return 0;
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
