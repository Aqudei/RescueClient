﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Models
{
    public class DownloadHouseholdModel : UploadHouseholdModel
    {
        public string Photo { get; set; }
        private List<DownloadPersonModel> _members;
        public List<DownloadPersonModel> members
        {
            get
            {
                return _members;
            }
            set
            {
                Set(ref _members, value);
            }
        }
        public DownloadHouseholdModel()
        {
            members = new List<DownloadPersonModel>();
        }
        public DownloadPersonModel FamilyHead
        {
            get
            {
                return members.Where(m => m.IsHead).FirstOrDefault();
            }
        }
    }
}
