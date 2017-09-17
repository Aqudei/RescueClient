using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RescueApp.ViewServices
{
    public class DialogService
    {
        private Window _container;

        public DialogService()
        {
            _container = new Window();

        }

        public string ShowOpenFileDialog()
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                return ofd.FileName;
            }
            return "";
        }
    }
}
