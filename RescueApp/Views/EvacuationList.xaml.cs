using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RescueApp.Views
{
    /// <summary>
    /// Interaction logic for EvacuationList.xaml
    /// </summary>
    public partial class EvacuationList : UserControl
    {
        public EvacuationList()
        {
            InitializeComponent();

            Messenger.Default.Register<Messages.NewCenterForMapMessage>(this, (loc) =>
            {
                //_mapControl.SetView(loc.Location, 12);


                //// Disables the default mouse double-click action.
                //// e.Handled = true;

                //// Determin the location to place the pushpin at on the map.

                ////Get the mouse click coordinates
                //Point mousePosition = e.GetPosition(this);
                ////Convert the mouse coordinates to a locatoin on the map
                //Location pinLocation = _mapControl.ViewportPointToLocation(mousePosition);

                // The pushpin to add to the map.

                //_mapControl.Children.Clear();

                //Pushpin pin = new Pushpin();
                //pin.Location = loc.Location;

                //// Adds the pushpin to the map.
                //_mapControl.Children.Add(pin);
                _mapControl.Markers.Clear();
                GMap.NET.PointLatLng latlong = new GMap.NET.PointLatLng(
                    loc.Location.Latitude, loc.Location.Longitude);

                GMapMarker marker = new GMapMarker(latlong);
                marker.Shape = new Ellipse
                {
                    Width = 10,
                    Height = 10,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.5
                };
                _mapControl.Markers.Add(marker);
                _mapControl.Position = latlong;
                _mapControl.Zoom = 8;
            });
        }

        private void GeneratingColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().ToLower().Contains("photo") ||
                e.Column.Header.ToString().ToLower().Equals("id"))

                e.Cancel = true;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { }

        private void _mapControl_Loaded(object sender, RoutedEventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            // choose your provider here
            _mapControl.MapProvider = GMap.NET.MapProviders.WikiMapiaMapProvider.Instance;
            _mapControl.MinZoom = 2;
            _mapControl.MaxZoom = 17;
            // whole world zoom
            _mapControl.Zoom = 5;
            // lets the map use the mousewheel to zoom
            _mapControl.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            // lets the user drag the map
            _mapControl.CanDragMap = true;
            // lets the user drag the map with the left mouse button
            _mapControl.DragButton = MouseButton.Left;
            _mapControl.SetPositionByKeywords("Eastern Visayas");
        }
    }
}
