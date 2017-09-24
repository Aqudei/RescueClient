using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Models
{
    public class UploadHouseholdModel : WithID
    {
        private string _address;

        public string Address
        {
            get { return _address; }
            set { Set(ref _address, value); }
        }

        public string EconomicStatus { get; set; }
        public string HouseNumber { get; set; }
        public bool IsOwned { get; set; }




    }

}
