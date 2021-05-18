using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DAL.DAO
{
    /// <summary>
    /// Interface that defines the Methods that should be provided for handling Tour Log DAOs
    /// </summary>
    public interface ILogDAO
    {
        /// <summary>
        /// Method that returns an item by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TourLog FindById(int id);
        /// <summary>
        /// Method that adds a new tour log item to the system of your choice. 
        /// </summary>
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
        /// <returns></returns>
        TourLog AddNewItemLog(Tour tourLogItem, string date, string totalTime, string report, int distance, int rating,
            int averageSpeed, int maxSpeed, int minSpeed, int averageStepCount, int burntCalories);
        /// <summary>
        /// Method that returns all tour logs of a tour from the system of your choice.
        /// </summary>
        /// <param name="tourItem"></param>
        /// <returns></returns>
        IEnumerable<TourLog> GetLogsForTour(Tour tourItem);
    }
}
