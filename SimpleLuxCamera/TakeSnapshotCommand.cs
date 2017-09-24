using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleLuxCamera
{
    public class TakeSnapshotCommand : ICommand
    {
        private Action takeSnapAction;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public TakeSnapshotCommand(Action takeSnapAction)
        {
            this.takeSnapAction = takeSnapAction;
        }
    }
}
