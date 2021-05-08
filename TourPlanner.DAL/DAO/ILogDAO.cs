using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.DAL.DAO
{
    public interface ILogDAO
    {
        TourLog FindById(int id);
        TourLog AddNewItemLog(Tour tourLogItem, string date, string totalTime, string report, double distance, int rating,
            int averageSpeed, int maxSpeed, int minSpeed, int averageStepCount, int burntCalories);
        IEnumerable<TourLog> GetLogsForTour(Tour tourItem);
    }
}
