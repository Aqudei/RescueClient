﻿using RescueApp.ViewModel;
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

namespace RescueApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            ViewModelLocator.Cleanup();
            Application.Current.Shutdown();
        }

        private void ShowReports(object sender, RoutedEventArgs e)
        {
            var rptWindow = new Views.Dialogs.Reporting();
            (rptWindow.DataContext as Views.Dialogs.ReportingVM).OnLoad();
            rptWindow.ShowDialog();
        }

        private void ShowSetting(object sender, RoutedEventArgs e)
        {
            var settingWindow = new Views.Dialogs.Settings();
            settingWindow.ShowDialog();
        }
    }
}
