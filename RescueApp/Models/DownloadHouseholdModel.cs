using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Models
{
    public class DownloadHouseholdModel : UploadHouseholdModel
    {

        private List<DownloadPersonModel> _members;
        private string _photo;

        public List<DownloadPersonModel> members
        {
            get
            {
                return _members;
            }
            set
            {
                Set(ref _members, value);
                RaisePropertyChanged(() => FamilyHead);
            }
        }

        public string Photo
        {
            get
            {
                return _photo;
            }
            set
            {
                Set(ref _photo, value);
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
