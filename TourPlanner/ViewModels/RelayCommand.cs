using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace TourPlanner.UI.ViewModels
{
    /// <summary>
    /// Class that solves the problem of having to create a new class for each command, because else the commands would not have different CanExcecute or Execute methods.
    /// To solve that problem there is the RelayCommandimplementation that is a command that can be instantiated passing the actions to be executed. 
    /// </summary>
    public class RelayCommand : ICommand
    {
        
        private Action<object> executeAction;
        private Func<object, bool> canExecutePredicate;

        /// <summary>
        /// customized Eventhandler for CanExecuteChanged event. 
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        /// <summary>
        /// Method to instantiate a Command.
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this.executeAction = execute;
            this.canExecutePredicate = canExecute;
        }
        /// <summary>
        /// Method to execute Command with the given parameter.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            executeAction(parameter);
        }
        /// <summary>
        /// Method to check if Command is executable. 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecutePredicate == null ? true : canExecutePredicate(parameter);
        }


    }
}
