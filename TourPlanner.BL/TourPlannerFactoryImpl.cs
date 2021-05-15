using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        public void Export()
        {
            List<ExportObject> exportObjects = new List<ExportObject>();
            List<TourLog> logs = new List<TourLog>();
            List<Tour> tours = new List<Tour>();
            tours = databaseTourItemDAO.GetItems();

            foreach (Tour tour in tours)
            {
                ExportObject exportObject = new ExportObject() { Tour = tour };
                exportObject.TourLogs = new List<TourLog>();
                if (databaseTourItemDAO.GetLogs(exportObject.Tour.Id).Count != 0)
                {
                    foreach (TourLog log in databaseTourItemDAO.GetLogs(exportObject.Tour.Id))
                    {
                        if (log != null)
                        {
                            exportObject.TourLogs.Add(log);
                        }
                    }
                }

                exportObjects.Add(exportObject);
            }
            filesystemTourItemDAO.Export(exportObjects);
        }

        public void Import(string fileName)
        {
            //WIP:make method to delete everything before
            //DeleteAllToursAndLogs();

            string json = File.ReadAllText(fileName);
            List<ExportObject> exportObjects = JsonConvert.DeserializeObject<List<ExportObject>>(json);

            foreach (ExportObject exportObject in exportObjects)
            {
                CreateTour(exportObject.Tour.Name, exportObject.Tour.From, exportObject.Tour.To, 
                    exportObject.Tour.Description, exportObject.Tour.RouteInformation,exportObject.Tour.Distance);
                if (exportObject.TourLogs != null)
                {
                    foreach (TourLog logItem in exportObject.TourLogs)
                    {
                        CreateTourLog(exportObject.Tour, logItem.Date, logItem.TotalTime, logItem.Report,
                                      logItem.Distance, logItem.Rating, logItem.AverageSpeed, logItem.MaxSpeed,
                                      logItem.MinSpeed, logItem.AverageStepCount, logItem.BurntCalories);
                    }
                }

            }
        }
    }
}
