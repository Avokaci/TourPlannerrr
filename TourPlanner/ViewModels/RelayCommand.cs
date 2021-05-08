using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace TourPlanner.UI.ViewModels
{
    public class RelayCommand : ICommand
    {
        private Action<object> executeAction;
        private Func<object, bool> canExecutePredicate;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this.executeAction = execute;
            this.canExecutePredicate = canExecute;
        }
        public void Execute(object parameter)
        {
            executeAction(parameter);
        }
        public bool CanExecute(object parameter)
        {
            return this.canExecutePredicate == null ? true : canExecutePredicate(parameter);
        }


    }
}
