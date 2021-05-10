using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DAL.Common
{
    public interface IFileAccess
    {
        int CreateNewTourFile(string name, string description, string from, string to, string routeInformation, int distance);
        int CreateNewLogFile(Tour tourLogItem, string date, string totalTime, string report, int distance, int rating,
            int averageSpeed, int maxSpeed, int minSpeed, int averageStepCount, int burntCalories);
        IEnumerable<FileInfo> SearchFiles(string searchTerm, MediaType searchType);

        IEnumerable<FileInfo> GetAllFiles(MediaType searchType);
    }
}
