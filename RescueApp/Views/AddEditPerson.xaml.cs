using Microsoft.Win32;
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
    /// Interaction logic for AddEditPerson.xaml
    /// </summary>
    public partial class AddEditPerson : UserControl
    {
        public AddEditPerson()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                txtPhoto.Text = ofd.FileName;
            }
        }
    }
}
