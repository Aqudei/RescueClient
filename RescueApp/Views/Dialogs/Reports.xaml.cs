using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using MahApps.Metro.Controls;
using RescueApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : MetroWindow
    {
        public Reports()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ((ReportsVM)DataContext).OnShown();
        }

        private void buttonPeopleReports_Click(object sender, RoutedEventArgs e)
        { }
    }
}
