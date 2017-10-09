using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RescueApp.Views.Dialogs
{
    public class SettingsVM : ViewModelBase
    {

        public int[] BaudRates => new int[] {
            4200,9600,19200
        };

        public string[] PortNames => new string[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9",
            "COM10",
            "COM11",
            "COM12",
            "COM13",
            "COM14",
            "COM15",
            "COM16",
            "COM17",
            "COM18",
            "COM19",
        };

        public int BaudRate
        {
            get
            {
                return Properties.Settings.Default.SMS_BAUD;
            }
            set
            {
                Properties.Settings.Default.SMS_BAUD = value;
                RaisePropertyChanged(nameof(BaudRate));
            }
        }

        public string COMPort
        {
            get
            {
                return Properties.Settings.Default.SMS_PORT;
            }
            set
            {
                Properties.Settings.Default.SMS_PORT = value;
                RaisePropertyChanged(nameof(COMPort));
            }
        }

        private RelayCommand saveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new RelayCommand(() =>
                {
                    Properties.Settings.Default.Save();
                }));
            }
        }

        public SettingsVM()
        {

        }

    }
}
