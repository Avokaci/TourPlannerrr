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
    /// TourItemFileDAO class that is responsible for filesystem interaction regarding tours. 
    /// </summary>
    public class TourItemFileDAO : ITourDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IFileAccess fileAccess;
        /// <summary>
        /// Constructor of class TourItemFileDAO
        /// </summary>
        public TourItemFileDAO()
        {
            this.fileAccess = DALFactory.GetFileAccess();
        }
        /// <summary>
        /// Method to add a new tour item to the filesystem. Takes the attributes of the tour as parameters. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="routeInformation"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public Tour AddNewItem(string name, string description, string from, string to, string routeInformation, int distance)
        {
            int id = fileAccess.CreateNewTourFile(name, description, from, to, routeInformation, distance);
            return FindById(id);
        }
        /// <summary>
        /// Method that finds a tour by its id in the filesystem and returns it. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public Tour FindById(int id)
        {
            IEnumerable<FileInfo> foundFiles = fileAccess.SearchFiles(id.ToString(), MediaType.Tour);
            return QuaryTourItemFromFileSystem(foundFiles).FirstOrDefault();

        }
        /// <summary>
        /// Method that returns all tours from the filesystem. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tour> GetTours()
        {
            IEnumerable<FileInfo> foundFiles = fileAccess.GetAllFiles(MediaType.Tour);
            return QuaryTourItemFromFileSystem(foundFiles);
        }
        /// <summary>
        /// Method that creates a tour object from the filesystem. 
        /// </summary>
        /// <param name="foundFiles"></param>
        /// <returns></returns>
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
                log.Info("Succesfully queried tours from filesystem");

            }
            catch (Exception ex)
            {

                log.Error("Could not query tours from file system " + ex.Message);
            }

            return foundTourItems;
        }
    }
}
