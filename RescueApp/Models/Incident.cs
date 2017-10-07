using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Models
{
    public class Incident : WithID
    {
        private string _incidentName;

        public string IncidentName
        {
            get { return _incidentName; }
            set { Set(ref _incidentName, value); }
        }

        private DateTime? _dateOccured;

        public DateTime? DateOccured
        {
            get { return _dateOccured; }
            set { Set(ref _dateOccured, value); }
        }


        private string _photo;

        public string Photo
        {
            get { return _photo; }
            set { Set(ref _photo, value); }
        }

        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            set { Set(ref _isActive, value); }
        }

        public override string ToString()
        {
            return IncidentName;
        }
    }
}
