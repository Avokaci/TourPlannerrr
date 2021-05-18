using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TourPlanner.DAL.Common;
using TourPlanner.DAL.DAO;
using TourPlanner.Models;

namespace TourPlanner.DAL.FileServer
{
    /// <summary>
    /// TourLogFileDAO class that is responsible for filesystem interaction regarding tour logs. 
    /// </summary>
    public class TourLogFileDAO : ILogDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IFileAccess fileAccess;
        private ITourDAO tourDAO;
        /// <summary>
        /// Constructor of class TourLogFileDAO.
        /// </summary>
        public TourLogFileDAO()
        {
            this.fileAccess = DALFactory.GetFileAccess();
            this.tourDAO = DALFactory.CreateTourItemDAO();
        }

        /// <summary>
        /// Method to add a new tour log item to the filesystem. Takes the attributes of the tour log as parameters. 
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
        public TourLog AddNewItemLog(Tour tourLogItem, string date, string totalTime, string report, int distance, int rating, int averageSpeed, int maxSpeed, int minSpeed, int averageStepCount, int burntCalories)
        {
            int id = fileAccess.CreateNewLogFile(tourLogItem, date, totalTime, report, distance, rating, averageSpeed, maxSpeed, minSpeed, averageStepCount, burntCalories);
            return FindById(id);
        }
        /// <summary>
        /// Method that finds a tour log by its id in the filesystem. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TourLog FindById(int id)
        {
            IEnumerable<FileInfo> foundFiles = fileAccess.SearchFiles(id.ToString(), MediaType.Log);
            return QueryTourLogFromFileSystem(foundFiles).FirstOrDefault();
        }
        /// <summary>
        /// Method that returns all logs of a tour from the filesystem
        /// </summary>
        /// <param name="tourItem"></param>
        /// <returns></returns>
        public IEnumerable<TourLog> GetLogsForTour(Tour tourItem)
        {
            IEnumerable<FileInfo> foundFiles = fileAccess.SearchFiles(tourItem.Id.ToString(), MediaType.Log);
            return QueryTourLogFromFileSystem(foundFiles); 
        }
        /// <summary>
        /// Method that creates a tour log object from the filesystem. 
        /// </summary>
        /// <param name="foundFiles"></param>
        /// <returns></returns>
        private IEnumerable<TourLog> QueryTourLogFromFileSystem(IEnumerable<FileInfo>foundFiles)
        {
            List<TourLog> foundTourLogs = new List<TourLog>();
            try
            {
                foreach (FileInfo file in foundFiles)
                {
                    string[] fileLines = File.ReadAllLines(file.FullName);
                    foundTourLogs.Add(new TourLog(
                        int.Parse(fileLines[0]),
                        tourDAO.FindById(int.Parse(fileLines[1])),
                        fileLines[2],
                        fileLines[3],
                        fileLines[4],
                        int.Parse(fileLines[5]),
                        int.Parse(fileLines[6]),
                        int.Parse(fileLines[7]),
                        int.Parse(fileLines[8]),
                        int.Parse(fileLines[9]),
                        int.Parse(fileLines[10]),
                        int.Parse(fileLines[11])
                    ));
                }
                log.Info("Succesfully queried tour logs from filesystem");

            }
            catch (Exception ex)
            {

                log.Error("Could not query tour logs from file system from tour " + ex.Message);
            }
           
            return foundTourLogs;
        }
    }
}
