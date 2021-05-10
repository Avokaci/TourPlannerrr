using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TourPlanner.DAL.Common;
using TourPlanner.Models;

namespace TourPlanner.DAL.FileServer
{
    public class FileAccess : IFileAccess
    {
        private string filePath;
        public FileAccess(string filePath)
        {
            this.filePath = filePath;
        }
        private IEnumerable<FileInfo> GetFileInfos(string startFolder,MediaType searchType)
        {
            DirectoryInfo dir = new DirectoryInfo(startFolder);
            return dir.GetFiles("*" + searchType.ToString() + ".txt", SearchOption.AllDirectories);
        }
        private string GetFullPath(string fileName)
        {
            return Path.Combine(filePath, fileName);
        }
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

       

        public IEnumerable<FileInfo> GetAllFiles(MediaType searchType)
        {
            return GetFileInfos(filePath, searchType);
        }

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
    }
}
