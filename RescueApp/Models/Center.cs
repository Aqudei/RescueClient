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
    }
}
