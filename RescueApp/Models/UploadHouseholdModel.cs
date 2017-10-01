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
        private string _houseCategory;
        private string _householdNumber;

        private string _houseOwnership;

        public string HouseOwnership
        {
            get { return _houseOwnership; }
            set { Set(ref _houseOwnership , value); }
        }


        public string Address
        {
            get { return _address; }
            set { Set(ref _address, value); }
        }

        public string HouseCategory
        {
            get { return _houseCategory; }
            set
            {
                Set(ref _houseCategory, value);
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

        private bool _isSafeZone;

        public bool IsSafeZone
        {
            get { return _isSafeZone; }
            set { Set(ref _isSafeZone, value); }
        }

        private bool _isTsunamiProne;

        public bool IsTsunamiProne
        {
            get { return _isTsunamiProne; }
            set { Set(ref _isTsunamiProne, value); }
        }


        private bool _isEarthquakeProne;

        public bool IsEarthquakeProne
        {
            get { return _isEarthquakeProne; }
            set { Set(ref _isEarthquakeProne, value); }
        }


        private bool _isFloodProne;

        public bool IsFloodProne
        {
            get { return _isFloodProne; }
            set { Set(ref _isFloodProne, value); }
        }

        private bool _isStormSurgeProne;

        public bool IsStormSurgeProne
        {
            get { return _isStormSurgeProne; }
            set { Set(ref _isStormSurgeProne, value); }
        }
    }

}
