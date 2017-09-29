using Microsoft.Maps.MapControl.WPF;
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
using System.Windows.Shapes;

namespace RescueApp.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for LocationPicker.xaml
    /// </summary>
    public partial class LocationPicker
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public LocationPicker()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void MapWithPushpins_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Disables the default mouse double-click action.
            e.Handled = true;

            // Determin the location to place the pushpin at on the map.

            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(this);
            //Convert the mouse coordinates to a locatoin on the map
            Location pinLocation = _mapControl.ViewportPointToLocation(mousePosition);

            // The pushpin to add to the map.
            Pushpin pin = new Pushpin();
            pin.Location = pinLocation;

            // Adds the pushpin to the map.
            _mapControl.Children.Add(pin);
        }

        private void _mapControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Disables the default mouse double-click action.
            // e.Handled = true;

            // Determin the location to place the pushpin at on the map.

            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(this);
            //Convert the mouse coordinates to a locatoin on the map
            Location pinLocation = _mapControl.ViewportPointToLocation(mousePosition);

            // The pushpin to add to the map.

            _mapControl.Children.Clear();

            Pushpin pin = new Pushpin();
            pin.Location = pinLocation;
            Latitude = pinLocation.Latitude;
            Longitude = pinLocation.Longitude;

            // Adds the pushpin to the map.
            _mapControl.Children.Add(pin);
        }
    }
}
