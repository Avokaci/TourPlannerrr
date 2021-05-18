using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BL;
using TourPlanner.Models;

namespace TourPlanner.UI.ViewModels
{
    /// <summary>
    /// AddLogViewModel class that is responsible for the creation of tour logs. 
    /// </summary>
    public class AddLogViewModel:BaseViewModel
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region instances
        private Tour currentTour;
        private string date;
        private string totalTime;
        private string report;
        private int distance;
        private int rating;
        private int averageSpeed;
        private int maxSpeed;
        private int minSpeed;
        private int averageStepCount;
        private int burntCalories;
        private ITourPlannerFactory tourPlannerFactory;
        private ICommand addCommand;
        public ICommand AddCommand => addCommand ??= new RelayCommand(AddLog);

        #endregion

        #region properties
        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                if (date != value)
                {
                    date = value;
                    RaisePropertyChangedEvent(nameof(date));
                }
            }
        }
        public string TotalTime
        {
            get
            {
                return totalTime;
            }
            set
            {
                if (totalTime != value)
                {
                    totalTime = value;
                    RaisePropertyChangedEvent(nameof(totalTime));
                }
            }
        }
        public string Report
        {
            get
            {
                return report;
            }
            set
            {
                if (report != value)
                {
                    report = value;
                    RaisePropertyChangedEvent(nameof(report));
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
        public int Rating
        {
            get
            {
                return rating;
            }
            set
            {
                if (rating != value)
                {
                    rating = value;
                    RaisePropertyChangedEvent(nameof(rating));
                }
            }
        }
        public int AverageSpeed
        {
            get
            {
                return averageSpeed;
            }
            set
            {
                if (averageSpeed != value)
                {
                    averageSpeed = value;
                    RaisePropertyChangedEvent(nameof(averageSpeed));
                }
            }
        }
        public int MaxSpeed
        {
            get
            {
                return maxSpeed;
            }
            set
            {
                if (maxSpeed != value)
                {
                    maxSpeed = value;
                    RaisePropertyChangedEvent(nameof(maxSpeed));
                }
            }
        }
        public int MinSpeed
        {
            get
            {
                return minSpeed;
            }
            set
            {
                if (minSpeed != value)
                {
                    minSpeed = value;
                    RaisePropertyChangedEvent(nameof(minSpeed));
                }
            }
        }
        public int AverageStepCount
        {
            get
            {
                return averageStepCount;
            }
            set
            {
                if (averageStepCount != value)
                {
                    averageStepCount = value;
                    RaisePropertyChangedEvent(nameof(averageStepCount));
                }
            }
        }
        public int BurntCalories
        {
            get
            {
                return burntCalories;
            }
            set
            {
                if (burntCalories != value)
                {
                    burntCalories = value;
                    RaisePropertyChangedEvent(nameof(burntCalories));
                }
            }
        }

        public Tour CurrentTour 
        {
            get 
            { 
                return currentTour;
            }
            set
            {
                if ((currentTour != value) && (value != null))
                {
                    currentTour = value;
                    RaisePropertyChangedEvent(nameof(CurrentTour));
                }
            }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Constructor of AddLogViewModel class
        /// </summary>
        public AddLogViewModel()
        {
            this.tourPlannerFactory = TourPlannerFactory.GetInstance();
        }

        #endregion

        #region methods 
        /// <summary>
        /// Method that allows for a log to be added to an existing tour. 
        /// </summary>
        /// <param name="commandParameter"></param>
        private void AddLog(object commandParameter)
        {

            try
            {
                TourLog generatedLog = tourPlannerFactory.CreateTourLog(CurrentTour, Date, TotalTime, Report, Distance, Rating, AverageSpeed, MaxSpeed, MinSpeed, AverageStepCount, BurntCalories);
                //tours.add(generatedLog)
                log.Info("Log succesfully added for tour " +currentTour.Name +" with id "  + currentTour.Id + " into tour log list");

            }
            catch (Exception ex)
            {

                log.Error("Could not add Log to tour " + currentTour.Name + " " + ex.Message);
            }

         

        }

        #endregion
    }
}
