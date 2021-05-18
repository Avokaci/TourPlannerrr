using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DAL.Common
{
    /// <summary>
    /// Interface that creates the Method heads for the interaction with the filesystem.
    /// </summary>
    public interface IFileAccess
    {
        /// <summary>
        /// Method that creates a new tour file with the tour attributes as a parameter. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="routeInformation"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        int CreateNewTourFile(string name, string description, string from, string to, string routeInformation, int distance);
        /// <summary>
        /// Method that creates a new tour log file with the tour log attributes as a parameter. 
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
        int CreateNewLogFile(Tour tourLogItem, string date, string totalTime, string report, int distance, int rating,
            int averageSpeed, int maxSpeed, int minSpeed, int averageStepCount, int burntCalories);
        /// <summary>
        /// Method that allows for the searching of specific files in the filesystem via searchterm and searchtype. 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        IEnumerable<FileInfo> SearchFiles(string searchTerm, MediaType searchType);
        /// <summary>
        /// Method to retrieve all files from the filesystem
        /// </summary>
        /// <param name="searchType"></param>
        /// <returns></returns>
        IEnumerable<FileInfo> GetAllFiles(MediaType searchType);
        public string CreateImage(string from, string to, string routeInformation);
    }
}
