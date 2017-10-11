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

        private string incidentType;

        public string IncidentType
        {
            get { return incidentType; }
            set { Set(ref incidentType, value); }
        }

        private DateTime? dateFinished;

        public DateTime? DateFinished
        {
            get { return dateFinished; }
            set { Set(ref dateFinished, value); }
        }


        private string earthquakeMagnitude;

        public string EarthquakeMagnitude
        {
            get { return earthquakeMagnitude; }
            set { Set(ref earthquakeMagnitude, value); }
        }


        private string earthquakeEpicenter;

        public string EarthquakeEpicenter
        {
            get { return earthquakeEpicenter; }
            set { Set(ref earthquakeEpicenter, value); }
        }

        private string typhoonSignal;

        public string TyphoonSignal
        {
            get { return typhoonSignal; }
            set { Set(ref typhoonSignal, value); }
        }
    }
}
