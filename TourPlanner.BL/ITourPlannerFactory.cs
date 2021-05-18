using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.BL
{
    /// <summary>
    /// Interface to define the structure of the functionalities/methods regarding the tours and tourlogs. Factory Method is a creational design 
    /// pattern that provides an interface for creating objects in a superclass, but allows subclasses to alter the type of objects that will be created.
    /// </summary>
    public interface ITourPlannerFactory
    {
        /// <summary>
        /// Method to retrieve information about all the tours. 
        /// </summary>
        /// <returns></returns>
        IEnumerable<Tour> GetTours();
        /// <summary>
        /// Method to retrieve information about all the tour logs that are appointed to the same tour. 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        IEnumerable<TourLog> GetLogs(Tour item);
        /// <summary>
        /// Method to provide the search function to find a specific tour in a list of tours / in the GUI
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        IEnumerable<Tour> Search(string itemName, bool caseSensitive = false);
        /// <summary>
        /// Method that creates a tour item with the tour attributes as parameters. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="routeInformation"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        Tour CreateTour(string name, string description, string from, string to, string routeInformation, int distance);
        /// <summary>
        /// Method that creates a tour log for a tour item with the tour id and tour log attributes as parameters. 
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
        TourLog CreateTourLog(Tour tourLogItem, string date, string totalTime, string report, int distance, int rating,
            int averageSpeed, int maxSpeed, int minSpeed, int averageStepCount, int burntCalories);
        /// <summary>
        /// Method that allows for export of all the tours with their tour logs into a file. 
        /// </summary>
        public void Export();
        /// <summary>
        /// Method that allows for import of all the tour with their tour logs from a file. 
        /// </summary>
        /// <param name="fileName"></param>
        public void Import(string fileName);


    }
}
