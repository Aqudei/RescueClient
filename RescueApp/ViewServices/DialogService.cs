using Microsoft.Win32;
using RescueApp.Views;
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
        private DialogHost _container;
        private Dictionary<string, Type> _dialogs;

        public DialogService()
        {
            _container = new DialogHost();
            _container.Closing += (s, e) =>
            {
                e.Cancel = true;
                _container.Hide();
            };
            _dialogs = new Dictionary<string, Type>();
        }

        public void RegisterDialog<T>(string dialogName)
        {
            if (_dialogs.ContainsKey(dialogName))
                return;

            _dialogs.Add(dialogName, typeof(T));
        }

        public void ShowDialog(string dialogName)
        {
            if (_dialogs.ContainsKey(dialogName) == false)
                return;

            var view = Activator.CreateInstance(_dialogs[dialogName]);
            _container.Content = view;
            _container.ShowDialog();
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
