using System;
using System.Collections.Generic;
using System.Text;

namespace TourPlanner.Models
{
    /// <summary>
    /// TourLog item class. Includes class attributes, constructor and access modifiers. 
    /// </summary>
    public class TourLog
    {
        private int id;
        private Tour tourLogItem;
        private string date;
        private string totalTime;
        private string report;
        private int distance;
        private int rating;
        //additional properties
        private int averageSpeed;
        private int maxSpeed;
        private int minSpeed;
        private int averageStepCount;
        private int burntCalories;

        /// <summary>
        /// Constructor of class which intantiates a TourLog item. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tourLogItem"></param>
        /// <param name="date"></param>
        /// <param name="totalTime"></param>
        /// <param name="report"></param>
        /// <param name="distance"></param>
        /// <param name="rating"></param>
        /// <param name="averageSpeed"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="minSpeed"></param>
        /// <param name="averageStepCount"></param>
        /// <param name="burntCalories"></param>
        public TourLog(int id, Tour tourLogItem, string date, string totalTime, string report, int distance,
            int rating, int averageSpeed, int maxSpeed, int minSpeed, int averageStepCount, int burntCalories)
        {
            this.id = id;
            this.tourLogItem = tourLogItem;
            this.date = date;
            this.totalTime = totalTime;
            this.report = report;
            this.distance = distance;
            this.rating = rating;
            this.averageSpeed = averageSpeed;
            this.maxSpeed = maxSpeed;
            this.minSpeed = minSpeed;
            this.averageStepCount = averageStepCount;
            this.burntCalories = burntCalories;
        }

        public int Id { get => id; set => id = value; }
        public Tour TourLogItem { get => tourLogItem; set => tourLogItem = value; }
        public string Date { get => date; set => date = value; }
        public string TotalTime { get => totalTime; set => totalTime = value; }
        public string Report { get => report; set => report = value; }
        public int Distance { get => distance; set => distance = value; }
        public int Rating { get => rating; set => rating = value; }
        public int AverageSpeed { get => averageSpeed; set => averageSpeed = value; }
        public int MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
        public int MinSpeed { get => minSpeed; set => minSpeed = value; }
        public int AverageStepCount { get => averageStepCount; set => averageStepCount = value; }
        public int BurntCalories { get => burntCalories; set => burntCalories = value; }
    }
}
