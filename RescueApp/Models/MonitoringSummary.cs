﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Models
{
    public class MonitoringSummary
    {
        public int num_members { get; set; }
        public Center center { get; set; }
        public int num_evacuees { get; set; }
        public int num_evacuated
        {
            get
            {
                return num_evacuees;
            }
        }
    }
}
