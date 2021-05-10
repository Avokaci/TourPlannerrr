using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TourPlanner.DAL.Common;
using TourPlanner.DAL.DAO;
using TourPlanner.Models;

namespace TourPlanner.BL
{
    internal class TourPlannerFactoryImpl : ITourPlannerFactory
    {
        public IEnumerable<Tour> GetTours()
        {
            ITourDAO tourDAO = DALFactory.CreateTourItemDAO();
            return tourDAO.GetTours();
        }
        public IEnumerable<TourLog> GetLogs(Tour item)
        {
            ILogDAO logDAO = DALFactory.CreateTourLogDAO();
            return logDAO.GetLogsForTour(item);
        }

        public IEnumerable<Tour> Search(string itemName, bool caseSensitive = false)
        {
            IEnumerable<Tour> items = GetTours();

            if (caseSensitive)
            {
                return items.Where(x => x.Name.Contains(itemName));
            }
            else
            {
                return items.Where(x => x.Name.ToLower().Contains(itemName.ToLower()));
            }
        }

        public Tour CreateTour(string name, string description, string from, string to, string routeInformation, int distance)
        {
            ITourDAO tourDAO = DALFactory.CreateTourItemDAO();
            return tourDAO.AddNewItem(name, description, from, to, routeInformation, distance);
        }

        public TourLog CreateTourLog(Tour tourLogItem, string date, string totalTime, string report, int distance, int rating,
            int averageSpeed, int maxSpeed, int minSpeed, int averageStepCount, int burntCalories)
        {
            ILogDAO logDAO = DALFactory.CreateTourLogDAO();
            return logDAO.AddNewItemLog(tourLogItem, date, totalTime, report, distance, rating, averageSpeed, maxSpeed, minSpeed, averageStepCount, burntCalories);
        }


    }
}
