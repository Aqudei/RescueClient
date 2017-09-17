using GalaSoft.MvvmLight.Messaging;
using RescueApp.Messages;
using RescueApp.Models;
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
    /// Interaction logic for People.xaml
    /// </summary>
    public partial class People : UserControl
    {
        public People()
        {
            InitializeComponent();

            Messenger.Default.Register<AddEditMessage<Person>>(this, (p) =>
            {
                var frm = new AddEditPerson();
                (frm.DataContext as AddEditPersonVM).Person = p.Entity;
                var win = new Window();
                win.Content = frm;
                win.ShowDialog();
            });
        }
    }
}
