using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ManagerPanel
{
    public class RelayCommand : ICommand
    {
        Action _action;
        Predicate<object> _canExecute;

        public RelayCommand(Action execute)
            : this(execute, null)
        {}
        public RelayCommand(Action execute, Predicate<object> canExecute )
        {
            _action = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null) return true;
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}