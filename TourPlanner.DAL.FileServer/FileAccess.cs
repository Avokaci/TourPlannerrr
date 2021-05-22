using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using TourPlanner.DAL.Common;
using TourPlanner.Models;

namespace TourPlanner.DAL.FileServer
{
    /// <summary>
    /// FileAccess class, which is responsible for all the interaction with the filesystem. The purpose of this class is to get file infos, get the full path of a file
    /// where a object is saved in the filesystem, creating a new tour file, creating a new log file, getting all files, searching in the filesystem for a specific file,
    /// getting a file text and creating a image for a tour via integration of the mapquest API. 
    /// </summary>
    public class FileAccess : IFileAccess
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string filePath;
        /// <summary>
        /// Constructor of the FileAccess class which takes a filePath as a parameter. 
        /// </summary>
        /// <param name="filePath"></param>
        public FileAccess(string filePath)
        {
            this.filePath = filePath;
        }
        /// <summary>
        /// Method to get the file infos of a specific file. 
        /// </summary>
        /// <param name="startFolder"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        private IEnumerable<FileInfo> GetFileInfos(string startFolder,MediaType searchType)
        {
            DirectoryInfo dir = new DirectoryInfo(startFolder);
            return dir.GetFiles("*" + searchType.ToString() + ".txt", SearchOption.AllDirectories);
        }
        /// <summary>
        /// Method that returns the fullpath of a file by its filename.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetFullPath(string fileName)
        {
            return Path.Combine(filePath, fileName);
        }
        /// <summary>
        /// Method that creates a new tour file in the filesystem. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="routeInformation"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public int CreateNewTourFile(string name, string description, string from, string to, string routeInformation, int distance)
        {
            int id = Guid.NewGuid().GetHashCode();
            string fileName = id + "_TourItem.txt";
            string path = GetFullPath(fileName);
            using (StreamWriter writer = File.CreateText(path))
            {
                writer.WriteLine(id);
                writer.WriteLine(name);
                writer.WriteLine(description);
                writer.WriteLine(from);
                writer.WriteLine(to);
                writer.WriteLine(routeInformation);
                writer.WriteLine(distance);

            }
            return id;
        }
        /// <summary>
        /// Method that creates a new tour log file in the filesystem. 
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
        public int CreateNewLogFile(Tour tourLogItem, string date, string totalTime, string report, int distance, int rating, int averageSpeed, int maxSpeed, int minSpeed, int averageStepCount, int burntCalories)
        {
            int id = Guid.NewGuid().GetHashCode();
            string fileName = id + "_TourLog.txt";
            string path = GetFullPath(fileName);
            using (StreamWriter writer = File.CreateText(path))
            {
                writer.WriteLine(id);
                writer.WriteLine(tourLogItem.Id);
                writer.WriteLine(date);
                writer.WriteLine(totalTime);
                writer.WriteLine(report);
                writer.WriteLine(distance);
                writer.WriteLine(rating);
                writer.WriteLine(averageSpeed);
                writer.WriteLine(rating);
                writer.WriteLine(maxSpeed);
                writer.WriteLine(minSpeed);
                writer.WriteLine(averageStepCount);
                writer.WriteLine(burntCalories);

            }
            return id;
        }

       
        /// <summary>
        /// Method that retrieves all the all files in the filesystem. 
        /// </summary>
        /// <param name="searchType"></param>
        /// <returns></returns>
        public IEnumerable<FileInfo> GetAllFiles(MediaType searchType)
        {
            return GetFileInfos(filePath, searchType);
        }

        /// <summary>
        /// Method that allows for searching for a specific searchterm and searchtype
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        public IEnumerable<FileInfo> SearchFiles(string searchTerm, MediaType searchType)
        {
            IEnumerable<FileInfo> fileList = GetFileInfos(filePath, searchType);
            IEnumerable<FileInfo> queryMatchingFiles =
                from file in fileList
                let fileText = GetFileText(file)
                where fileText.Contains(searchTerm)
                select file;
            return queryMatchingFiles;
                
        }
        /// <summary>
        /// Method that gets the text content of a file. 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string GetFileText(FileInfo file)
        {
            using StreamReader reader = file.OpenText();
            StringBuilder sb = new StringBuilder();
            while (!reader.EndOfStream)
            {
                sb.Append(reader.ReadLine());
            }
            return sb.ToString();
        }
        /// <summary>
        /// Method that is responsible for creating an image from the provided start and end location via integration of the MapQuest API. 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="routeInformation"></param>
        /// <returns></returns>
        public string CreateImage(string from, string to, string routeInformation)
        {
            string key = "tTz9lIg7C5SWXJsGlSgT6yRST3mjerGR";
            string imageFilePath;
            string url = @"https://www.mapquestapi.com/staticmap/v5/map?start=" + from + "&end=" + to + "&size=600,400@2x&key=" + key;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse lxResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream()))
                    {
                        Byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
                        Random rnd = new Random();
                        imageFilePath = filePath + routeInformation + ".jpg";
                        using (FileStream lxFS = new FileStream(imageFilePath, FileMode.Create))
                        {
                            lxFS.Write(lnByte, 0, lnByte.Length);
                        }
                    }
                }
                return imageFilePath;
            }
            catch (Exception ex)
            {

                log.Error("Image could not be generated " + ex.Message);
                throw;
            }
           
        }
    }
}
