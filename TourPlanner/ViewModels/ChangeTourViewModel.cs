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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string description;
        private Tour currentItem;
        private ObservableCollection<Tour> tours;
        private ITourPlannerFactory tourFactory;


        private ICommand changeCommand;
        public ICommand ChangeCommand => changeCommand ??= new RelayCommand(ChangeTour);

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
        public Tour CurrentItem
        {
            get
            {
                return currentItem;
            }
            set
            {
                if (currentItem != value)
                {
                    currentItem = value;
                    RaisePropertyChangedEvent(nameof(currentItem));
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
            try
            {

            }
            catch (Exception ex)
            {

                log.Error("Could not change tour description of tour " + currentItem.Name + " with id" + currentItem.Id + " " + ex.Message);
            }
        }
        #endregion
    }
}
