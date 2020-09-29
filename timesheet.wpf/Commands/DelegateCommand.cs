using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace timesheet.wpf.Commands
{
    public class DelegateCommand : ICommand
    {
        private Action _target;


        public DelegateCommand(Action target)
        {
            _target = target;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_target != null)
            {
                _target.Invoke();
                return;
            }
        }
    }
}
