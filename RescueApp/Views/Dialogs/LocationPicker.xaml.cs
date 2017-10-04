using GMap.NET.WindowsPresentation;
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
    public partial class LocationPicker : IDisposable
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
        { }

        private void _mapControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //// Disables the default mouse double-click action.
            //// e.Handled = true;

            //// Determin the location to place the pushpin at on the map.

            ////Get the mouse click coordinates
            //Point mousePosition = e.GetPosition(this);
            ////Convert the mouse coordinates to a locatoin on the map
            //Location pinLocation = _mapControl.ViewportPointToLocation(mousePosition);

            //// The pushpin to add to the map.

            //_mapControl.Children.Clear();

            //Pushpin pin = new Pushpin();
            //pin.Location = pinLocation;
            //Latitude = pinLocation.Latitude;
            //Longitude = pinLocation.Longitude;

            //// Adds the pushpin to the map.
            //_mapControl.Children.Add(pin);
        }

        private void _gmap_Loaded(object sender, RoutedEventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            // choose your provider here
            _gmap.MapProvider = GMap.NET.MapProviders.WikiMapiaMapProvider.Instance;
            _gmap.MinZoom = 2;
            _gmap.MaxZoom = 17;
            // whole world zoom
            _gmap.Zoom = 12;
            // lets the map use the mousewheel to zoom
            _gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            // lets the user drag the map
            _gmap.CanDragMap = true;
            // lets the user drag the map with the left mouse button
            _gmap.DragButton = MouseButton.Left;

            //@12.4112461,124.5903656,12
            _gmap.Position = new GMap.NET.PointLatLng(12.4112461, 124.5903656);

        }

        private void _gmap_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            _gmap.Markers.Clear();
            var pos = e.GetPosition(_gmap);
            var latlong = _gmap.FromLocalToLatLng((int)pos.X, (int)pos.Y);

            GMapMarker marker = new GMapMarker(latlong);
            marker.Shape = new Ellipse
            {
                Width = 16,
                Height = 16,
                Stroke = Brushes.Black,
                Fill = Brushes.Red,
                StrokeThickness = 1.5
            };
            _gmap.Markers.Add(marker);
            Latitude = marker.Position.Lat;
            Longitude = marker.Position.Lng;
        }

        public void Dispose()
        {
            _gmap.Dispose();
        }
    }
}
