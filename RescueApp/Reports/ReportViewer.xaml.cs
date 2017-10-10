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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SAPBusinessObjects.WPF.Viewer;

namespace RescueApp.Reports
{
    /// <summary>
    /// Interaction logic for ReportViewer.xaml
    /// </summary>
    public partial class ReportViewer : Window
    {
        private CrystalReportsViewer reportViewer;

        public ReportViewer()
        {
            InitializeComponent();

            reportViewer = new CrystalReportsViewer();
            reportViewwerContainer.Children.Add(reportViewer);
        }

        public ReportDocument ReportSource { get; internal set; }
        public Dictionary<string, object> Parameters { get; internal set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            reportViewer.ViewerCore.ReportSource = ReportSource;

            foreach (var item in Parameters)
            {
                ReportSource.SetParameterValue(item.Key, item.Value);
            }
        }
    }
}
