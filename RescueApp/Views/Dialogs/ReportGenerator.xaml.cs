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
    /// Interaction logic for ReportGenerator.xaml
    /// </summary>
    public partial class ReportGenerator : Window
    {
        public ReportGenerator()
        {
            InitializeComponent();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ((ReportGeneratorVM)DataContext).OnLoad();
        }
    }
}
