using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.UI.ViewModels
{
    public class AddLogViewModel:BaseViewModel
    {
        #region instances
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
        #endregion

        #region constructor

        #endregion

        #region methods 

        #endregion
    }
}
