using Microsoft.Win32;
using RescueApp.Interfaces;
using RescueApp.Views;
using RescueApp.Views.Dialogs;
using RescueApp.Views.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RescueApp.ViewServices
{
    public class DialogService
    {
        private DialogHost _container;
        private Dictionary<string, Type> _dialogs;
        private Window _cam;

        public DialogService()
        {
            _container = new DialogHost();
            _container.Closing += (s, e) =>
            {
                e.Cancel = true;
                if (_container._container.Children.Count > 0)
                {
                    var fe = _container._container.Children?[0] as FrameworkElement;
                    var vm = fe?.DataContext as PageBase;
                    vm?.DoCleanup();
                    _container.Hide();
                }
            };
            _dialogs = new Dictionary<string, Type>();
        }

        public Tuple<double, double> ShowMapPicker()
        {
            var dlgMap = new LocationPicker();
            if (dlgMap.ShowDialog() == true)
            {
                return new Tuple<double, double>(dlgMap.Latitude, dlgMap.Longitude);
            }
            else
            {
                return new Tuple<double, double>(0, 0); ;
            }
        }

        public void RegisterDialog<T>(string dialogName)
        {
            if (_dialogs.ContainsKey(dialogName))
                return;

            _dialogs.Add(dialogName, typeof(T));
        }

        public void ShowCamera() 
        {
            _cam = _cam ?? new Camera();
            if ((_cam as Camera)?.CameraCount > 0)
                _cam.ShowDialog();
            else
            {
                MessageBox.Show("NO CAMERA FOUND");
            }
        }

        public void ShowDialog(string dialogName)
        {
            if (_dialogs.ContainsKey(dialogName) == false)
                return;

            var view = Activator.CreateInstance(_dialogs[dialogName]);
            _container._container.Children.Clear();
            _container._container.Children.Add(view as UIElement);
            _container.ShowDialog();
        }

        public void ShowDialog<T>(string dialogName, T old)
        {
            if (_dialogs.ContainsKey(dialogName) == false)
                return;

            var view = Activator.CreateInstance(_dialogs[dialogName]);
            _container._container.Children.Clear();
            _container._container.Children.Add(view as FrameworkElement);
            var editor = ((view as FrameworkElement).DataContext as IEditorDialog<T>);
            editor?.Edit(old);

            _container.ShowDialog();
        }

        public string ShowOpenFileDialog()
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                return ofd.FileName;
            }
            return null;
        }
    }
}
