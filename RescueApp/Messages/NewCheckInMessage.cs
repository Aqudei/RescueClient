﻿using RescueApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Messages
{
    public class NewCheckInMessage
    {
        public CheckInInfo CheckInInfo { get; set; }
    }
}
