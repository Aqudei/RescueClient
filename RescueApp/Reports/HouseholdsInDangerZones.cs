using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Reports
{
    public class HouseholdsInDangerZones
    {
        public string Address { get; set; }
        public string HouseCategory { get; set; }
        public string HouseNumber { get; set; }
        public string family_head { get; set; }
        public int num_fam { get; set; }
        public int num_vulnerable { get; set; }

        public bool IsSafeZone { get; set; }
        public bool IsTsunamiProne { get; set; }
        public bool IsEarthquakeProne { get; set; }
        public bool IsFloodProne { get; set; }
        public bool IsStormSurgeProne { get; set; }
        public string Tags
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (IsSafeZone)
                {
                    sb.Append("#safe");

                }

                if (IsTsunamiProne)
                {
                    sb.Append("#tsunami");

                }

                if (IsEarthquakeProne)
                {
                    sb.Append("#earthquake");

                }

                if (IsFloodProne)
                {
                    sb.Append("#flood");

                }
                if (IsStormSurgeProne)
                {
                    sb.Append("#stormsurge");
                }

                return sb.ToString();
            }
        }

        /*
          "IsSafeZone": true,
        "IsTsunamiProne": false,
        "IsEarthquakeProne": false,
        "IsFloodProne": false,
        "IsStormSurgeProne": false, 
        */
    }
}
