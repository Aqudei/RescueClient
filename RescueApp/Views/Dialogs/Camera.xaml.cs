using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RescueApp.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for Camera.xaml
    /// </summary>
    public partial class Camera
    {
        public Camera()
        {
            InitializeComponent();
            this.IsVisibleChanged += Camera_IsVisibleChanged;
            this.Closing += Camera_Closing;
            _luxCamera.InitializeCamera();
        }

        private void Camera_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            _luxCamera.EndCapturing();

            Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Camera_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {

                _luxCamera.StartCamera();
            }

        }

        private void _capture_Click(object sender, RoutedEventArgs e)
        {
            _luxCamera.CaptureImage();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Retry(object sender, RoutedEventArgs e)
        {
            _luxCamera.Resume();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Accept(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new CapturedPhotoEvenArgs
            {
                PhotoPath = _luxCamera.LastCapturePath
            });
            this.Close();
        }
    }
}
