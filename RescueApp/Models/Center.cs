using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Models
{
    public class Center : HasId
    {

        string _address;
        int _limit;
        string _photo;

        public string CenterName { get; set; }
        public string Address {
            get
            {
                return _address;
            }

            set
            {
                _address = value;
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
