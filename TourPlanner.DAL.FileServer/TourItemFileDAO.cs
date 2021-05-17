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
    public class TourItemFileDAO : ITourDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IFileAccess fileAccess;
        public TourItemFileDAO()
        {
            this.fileAccess = DALFactory.GetFileAccess();
        }
        public Tour AddNewItem(string name, string description, string from, string to, string routeInformation, int distance)
        {
            int id = fileAccess.CreateNewTourFile(name, description, from, to, routeInformation, distance);
            return FindById(id);
        }

        public Tour FindById(int id)
        {
            IEnumerable<FileInfo> foundFiles = fileAccess.SearchFiles(id.ToString(), MediaType.Tour);
            return QuaryTourItemFromFileSystem(foundFiles).FirstOrDefault();

        }

        public IEnumerable<Tour> GetTours()
        {
            IEnumerable<FileInfo> foundFiles = fileAccess.GetAllFiles(MediaType.Tour);
            return QuaryTourItemFromFileSystem(foundFiles);
        }

        private IEnumerable<Tour> QuaryTourItemFromFileSystem(IEnumerable<FileInfo>foundFiles)
        {
            List<Tour> foundTourItems = new List<Tour>();
            try
            {
                foreach (FileInfo file in foundFiles)
                {
                    string[] fileLines = File.ReadAllLines(file.FullName);
                    foundTourItems.Add(new Tour(
                        int.Parse(fileLines[0]),
                        fileLines[1],
                        fileLines[2],
                        fileLines[3],
                        fileLines[4],
                        fileLines[5],
                        int.Parse(fileLines[6])

                        ));

                }
                log.Info("Succesfully queried tours from database");

            }
            catch (Exception ex)
            {

                log.Error("Could not query tours from file system " + ex.Message);
            }

            return foundTourItems;
        }
    }
}
