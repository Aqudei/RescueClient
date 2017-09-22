using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Models
{
    public class Household : WithID
    {
        public String Address { get; set; }
        public string EconomicStatus { get; set; }
        public string HouseNumber { get; set; }
        public bool IsOwned { get; set; }
    }
}
