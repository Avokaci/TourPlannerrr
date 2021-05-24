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
    /// <summary>
    /// TourPlannerFactoryImpl class that implements that Methods defined in the ITourPlannerFactory interface. 
    /// </summary>
    internal class TourPlannerFactoryImpl : ITourPlannerFactory
    {
        /// <summary>
        /// Method to retrieve information about all the tours. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tour> GetTours()
        {
            ITourDAO tourDAO = DALFactory.CreateTourItemDAO();
            return tourDAO.GetTours();
        }
        /// <summary>
        /// Method to retrieve information about all the tour logs that are appointed to the same tour. 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IEnumerable<TourLog> GetLogs(Tour item)
        {
            ILogDAO logDAO = DALFactory.CreateTourLogDAO();
            return logDAO.GetLogsForTour(item);
        }
        /// <summary>
        /// Method to provide the search function to find a specific tour in a list of tours / in the GUI
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
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
        public Tour CreateTour(string name, string description, string from, string to, string routeInformation, int distance)
        {
            ITourDAO tourDAO = DALFactory.CreateTourItemDAO();
            return tourDAO.AddNewItem(name, description, from, to, routeInformation, distance);
        }
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
        public TourLog CreateTourLog(Tour tourLogItem, string date, string totalTime, string report, int distance, int rating,
            int averageSpeed, int maxSpeed, int minSpeed, int averageStepCount, int burntCalories)
        {
            ILogDAO logDAO = DALFactory.CreateTourLogDAO();
            return logDAO.AddNewItemLog(tourLogItem, date, totalTime, report, distance, rating, averageSpeed, maxSpeed, minSpeed, averageStepCount, burntCalories);
        }
        /// <summary>
        /// Method that allows to export a tour into a file. 
        /// </summary>
        public void Export(Tour tour)
        {       
        
            tour.RouteInformation = null;
            string path = Path.Join("C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\ExportedObjects", tour.Name + ".json");
            string json = JsonConvert.SerializeObject(tour);

            File.WriteAllTextAsync(path, json);
        }
        /// <summary>
        /// Method to export a tour with its logs into a file
        /// </summary>
        /// <param name="tour"></param>
        public void ExportWithLogs(Tour tour)
        {
          //WIP
        }
        /// <summary>
        /// Method that allows to import an exported tour. 
        /// </summary>
        /// <param name="fileName"></param>
        public void Import(string fileName)
        {
            //WIP:make method to delete everything before
            string json = File.ReadAllText(fileName);
            Tour exportedTour = JsonConvert.DeserializeObject<Tour>(json);
            CreateTour(exportedTour.Name, exportedTour.From, exportedTour.To,
                    exportedTour.Description, exportedTour.RouteInformation, exportedTour.Distance);
        }

        /// <summary>
        /// Method that allows for import of all the tour with their tour logs from a file. 
        /// </summary>
        /// <param name="fileName"></param>
        public void ImportWithLogs(string fileName)
        {
            //Sadly is not working :(
            //WIP:make method to delete everything before
            
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
