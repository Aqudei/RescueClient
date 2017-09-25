using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleLuxCamera
{
    public class CameraCommand : ICommand
    {
        private Action takeSnapAction;
        private Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public void Execute(object parameter)
        {
            if (_canExecute())
                takeSnapAction();
        }

        public CameraCommand(Action takeSnapAction, Func<bool> canExecute)
        {
            this.takeSnapAction = takeSnapAction;
            _canExecute = canExecute;

            CommandManager.RequerySuggested += CommandManager_RequerySuggested;
        }

        private void CommandManager_RequerySuggested(object sender, EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }
    }
}
