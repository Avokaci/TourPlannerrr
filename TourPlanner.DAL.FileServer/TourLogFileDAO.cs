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
    public class TourLogFileDAO : ILogDAO
    {
        private IFileAccess fileAccess;
        private ITourDAO tourDAO;
        public TourLogFileDAO()
        {
            this.fileAccess = DALFactory.GetFileAccess();
            this.tourDAO = DALFactory.CreateTourItemDAO();
        }

        public TourLog AddNewItemLog(Tour tourLogItem, string date, string totalTime, string report, int distance, int rating, int averageSpeed, int maxSpeed, int minSpeed, int averageStepCount, int burntCalories)
        {
            int id = fileAccess.CreateNewLogFile(tourLogItem, date, totalTime, report, distance, rating, averageSpeed, maxSpeed, minSpeed, averageStepCount, burntCalories);
            return FindById(id);
        }

        public TourLog FindById(int id)
        {
            IEnumerable<FileInfo> foundFiles = fileAccess.SearchFiles(id.ToString(), MediaType.Log);
            return QueryTourLogFromFileSystem(foundFiles).FirstOrDefault();
        }

        public IEnumerable<TourLog> GetLogsForTour(Tour tourItem)
        {
            IEnumerable<FileInfo> foundFiles = fileAccess.SearchFiles(tourItem.Id.ToString(), MediaType.Log);
            return QueryTourLogFromFileSystem(foundFiles); 
        }
        private IEnumerable<TourLog> QueryTourLogFromFileSystem(IEnumerable<FileInfo>foundFiles)
        {
            List<TourLog> foundTourLogs = new List<TourLog>();
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
            return foundTourLogs;
        }
    }
}
