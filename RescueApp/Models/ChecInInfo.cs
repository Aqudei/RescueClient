﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Models
{
    public class CheckInInfo
    {
        
        public int Id { get; set; }
        public string scope { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string status { get; set; }
    }
}
