using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Models
{
    public class Center : WithID
    {
        private string _address;
        private int _limit;
        private string _photo;

        private double _latitude;

        public double Latitude
        {
            get { return _latitude; }
            set { Set(ref _latitude, value); }
        }


        private double _longitude;

        public double Longitude
        {
            get { return _longitude; }
            set { Set(ref _longitude, value); }
        }


        public string CenterName { get; set; }
        public string Address
        {
            get
            {
                return _address;
            }

            set
            {
                Set(ref _address, value);
            }
        }
        public int Limit
        {
            get { return _limit; }
            set
            {
                _limit = value;
            }
        }
        public string Photo
        {
            get { return _photo; }
            set { _photo = value; }
        }

        private List<DownloadPersonModel> _members;

        public List<DownloadPersonModel> members
        {
            get { return _members; }
            set
            {
                Set(ref _members, value);
            }
        }

        private string _amenities;

        public string Amenities
        {
            get { return _amenities; }
            set { Set(ref _amenities, value); }
        }

        private string _inChargeCellphone;

        public string InChargeCellphone
        {
            get { return _inChargeCellphone; }
            set { Set(ref _inChargeCellphone, value); }
        }

        private string _inCharge;

        public string InCharge
        {
            get { return _inCharge; }
            set { Set(ref _inCharge, value); }
        }



    }
}
