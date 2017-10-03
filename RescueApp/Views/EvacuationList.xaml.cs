using GalaSoft.MvvmLight.Messaging;
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
                _mapControl.SetView(loc.Location, 12);


                //// Disables the default mouse double-click action.
                //// e.Handled = true;

                //// Determin the location to place the pushpin at on the map.

                ////Get the mouse click coordinates
                //Point mousePosition = e.GetPosition(this);
                ////Convert the mouse coordinates to a locatoin on the map
                //Location pinLocation = _mapControl.ViewportPointToLocation(mousePosition);

                // The pushpin to add to the map.

                _mapControl.Children.Clear();

                Pushpin pin = new Pushpin();
                pin.Location = loc.Location;

                // Adds the pushpin to the map.
                _mapControl.Children.Add(pin);

            });
        }

        private void GeneratingColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().ToLower().Contains("photo") ||
                e.Column.Header.ToString().ToLower().Equals("id"))

                e.Cancel = true;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
