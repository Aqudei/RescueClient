using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Models
{
    public class DownloadPersonModel : UploadPersonModel
    {
        public string Photo { get; set; }
        public int? _Center { get; set; }
    }
}
