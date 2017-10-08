using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Models
{
    public class PersonStatus
    {
        public int Person { get; set; }


        public int id
        {
            get
            {
                return Person;
            }
        }

        public string When { get; set; }
        public string Status { get; set; }
    }
}
