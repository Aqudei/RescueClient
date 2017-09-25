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
    /// Interaction logic for EvacuationList.xaml
    /// </summary>
    public partial class EvacuationList : UserControl
    {
        public EvacuationList()
        {
            InitializeComponent();
        }

        private void GeneratingColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().ToLower().Contains("photo") ||
                e.Column.Header.ToString().ToLower().Equals("id"))

                e.Cancel = true;

            
        }
    }
}
