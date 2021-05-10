using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BL;
using TourPlanner.Models;

namespace TourPlanner.UI.ViewModels
{
    public class AddTourViewModel : BaseViewModel
    {
        #region instances
        private string name;
        private string from;
        private string to;
        private string description;
        private int distance;

        private ITourPlannerFactory tourFactory;

        private ICommand addCommand;
        public ICommand AddCommand => addCommand ??= new RelayCommand(AddTour);
        #endregion

        #region properties
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChangedEvent(nameof(name));
                }
            }
        }
        public string From
        {
            get
            {
                return from;
            }
            set
            {
                if (from != value)
                {
                    from = value;
                    RaisePropertyChangedEvent(nameof(from));
                }
            }
        }
        public string To
        {
            get
            {
                return to;
            }
            set
            {
                if (to != value)
                {
                    to = value;
                    RaisePropertyChangedEvent(nameof(to));
                }
            }
        }
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (description != value)
                {
                    description = value;
                    RaisePropertyChangedEvent(nameof(description));
                }
            }
        }

        public int Distance
        {
            get
            {
                return distance;
            }
            set
            {
                if (distance != value)
                {
                    distance = value;
                    RaisePropertyChangedEvent(nameof(distance));
                }
            }
        }



        #endregion
        #region constructor
        public AddTourViewModel()
        {
            this.tourFactory = TourPlannerFactory.GetInstance();

        }
        #endregion
        #region methods
        private void AddTour(object commandParameter)
        {
            Tour addedTour = tourFactory.CreateTour(Name, Description, From, To, NameGenerator.GenerateName(5), Distance);
            var window = (Window)commandParameter;
            name = string.Empty;
            from = string.Empty;
            to = string.Empty;
            description = string.Empty;
            distance = 0;
            window.Close();

        }
        #endregion
    }
}
