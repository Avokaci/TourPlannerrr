using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TourPlanner.UI.ViewModels
{
    /// <summary>
    /// Abstract class that serves as a foundation for all the other viewModel classes that are going to inherit from it. 
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Method that raises an event when a property of the parameter has changed. 
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }


    }
}
