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
        public String Title { get; } = "Monitoring";

        public string BingKey { get; } = "An55arZpUhhCqmZEbjdMlDacaHE3nD0v6-N5442PY9urTYIj1vTiDq4N8S4OSYCy";

        public override void OnShow<T>(T args)
        { }
    }
}
