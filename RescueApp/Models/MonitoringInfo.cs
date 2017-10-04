using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Models
{
    public class MonitoringInfo
    {
        public Center center { get; set; }
        public List<PersonId> persons { get; set; }

        public MonitoringInfo()
        {
            persons = new List<PersonId>();
        }
    }
}
