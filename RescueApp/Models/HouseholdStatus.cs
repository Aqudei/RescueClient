using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Models
{
    // Use in monitoring
    public class HouseholdStatus
    {
        public int id { get; set; }
        public int num_fam { get; set; }
        public string Status { get; set; }
        public string Household { get; set; }
        public string family_head { get; set; }
    }
}
