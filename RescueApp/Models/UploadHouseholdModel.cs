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
        private string _economicStatus;
        private string _householdNumber;
        private bool _isOwned;

        public string Address
        {
            get { return _address; }
            set { Set(ref _address, value); }
        }

        public string EconomicStatus
        {
            get { return _economicStatus; }
            set
            {
                Set(ref _economicStatus, value);
            }
        }
        public string HouseNumber
        {
            get { return _householdNumber; }
            set
            {
                Set(ref _householdNumber, value);
            }
        }

        public bool IsOwned
        {
            get
            {
                return _isOwned;
            }
            set
            {
                Set(ref _isOwned, value);
            }
        }




    }

}
