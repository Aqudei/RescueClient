using GalaSoft.MvvmLight;
using RescueApp.Views.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views
{
    public class MonitoringVM : PageBase
    {
        public String Title { get; set; } = "Monitoring";

        public override void OnShow<T>(T args)
        {}
    }
}
