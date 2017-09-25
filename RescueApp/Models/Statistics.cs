using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Models
{
    public class Statistics : ObservableObject
    {
        string _NumberOfPerson;
        string _NumberOfHousehold;
        string _NumberOfEvacuation;
        string _NumberOfCalamities;

        public Statistics()
        {}

        public string NumberOfPerson
        {
            get => _NumberOfPerson; set => Set(ref _NumberOfPerson, value);
        }

        public string NumberOfHousehold { get => _NumberOfHousehold; set => Set(ref _NumberOfHousehold, value); }
        public string NumberOfEvacuation { get => _NumberOfEvacuation; set => Set(ref _NumberOfEvacuation, value); }
        public string NumberOfCalamities { get => _NumberOfCalamities; set => Set(ref _NumberOfCalamities, value); }
    }
}
