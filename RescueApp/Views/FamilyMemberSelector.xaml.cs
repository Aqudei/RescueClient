﻿using System;
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
    /// Interaction logic for FamilyMemberSelector.xaml
    /// </summary>
    public partial class FamilyMemberSelector : UserControl
    {


        public FamilyMemberSelector()
        {
            InitializeComponent();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void ssss(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
