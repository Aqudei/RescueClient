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
    /// Interaction logic for AddEditIncident.xaml
    /// </summary>
    public partial class AddEditIncident : UserControl
    {
        public AddEditIncident()
        {
            InitializeComponent();

            comboBoxDisasterType.SelectionChanged += (s, e) =>
            {
                var selectedDisasterType = comboBoxDisasterType.SelectedItem?.ToString().ToLower() ?? "";
                if (selectedDisasterType.Contains("typhoon"))
                {
                    additionalInfo.IsEnabled = true;
                    additionalInfo.SelectedIndex = 0;
                }

                else if (selectedDisasterType.Contains("earthquake"))
                {
                    additionalInfo.IsEnabled = true;
                    additionalInfo.SelectedIndex = 1;
                }

                else
                {
                    additionalInfo.SelectedIndex = 2;
                    additionalInfo.IsEnabled = false;
                }
            };
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
