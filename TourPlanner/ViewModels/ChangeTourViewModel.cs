using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TourPlanner.BL;
using TourPlanner.Models;

namespace TourPlanner.UI.ViewModels
{
    public class ChangeTourViewModel: BaseViewModel
    {
        private string description;
        private ObservableCollection<Tour> tours;
        private ITourPlannerFactory tourFactory;

        private ICommand addCommand;
        public ICommand AddCommand => addCommand ??= new RelayCommand(ChangeTour);

        #region properties
       
     
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

        public ObservableCollection<Tour> Tours { get => tours; set => tours = value; }
        #endregion

        #region constructor
        public ChangeTourViewModel()
        {
            this.tourFactory = TourPlannerFactory.GetInstance();
            tours = new ObservableCollection<Tour>();
            //logs = new ObservableCollection<TourLog>();
            foreach (Tour item in this.tourFactory.GetTours())
            {
                tours.Add(item);
            }
        }
        #endregion

        #region methods
        private void ChangeTour(object commandParameter)
        {
           
        }
        #endregion
    }
}
