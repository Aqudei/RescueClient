using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Messages
{
    public class NewCenterForMapMessage
    {
        public PointLatLng Location { get; set; }
    }
}
