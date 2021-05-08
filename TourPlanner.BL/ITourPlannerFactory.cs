using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.BL
{
    public interface ITourPlannerFactory
    {
        IEnumerable<Tour> GetTours();
        IEnumerable<TourLog> GetLogs(Tour item);
        IEnumerable<Tour> Search(string itemName, bool caseSensitive = false);

        Tour CreateTour(string name, string description, string from, string to, string routeInformation, double distance);

        TourLog CreateTourLog(Tour tourLogItem, string date, string totalTime, string report, double distance, int rating,
            int averageSpeed, int maxSpeed, int minSpeed, int averageStepCount, int burntCalories);
    }
}
