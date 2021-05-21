using NUnit.Framework;
using QuestPDF.Fluent;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using TourPlanner.BL;
using TourPlanner.DAL.Common;
using TourPlanner.DAL.FileServer;
using TourPlanner.Models;
using TourPlanner.UI.ViewModels;

namespace TourPlanner.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CheckTourInitializationCorrect()
        {
            Tour tour = new Tour(3, "Tour-1", "this is tour 1", "Graz", "Wien", "C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\Pictures\\Nyjisha.jpg", 350);
            Assert.IsNotNull(tour);
        }
        [Test]
        public void CheckTourLogInitializationCorrect()
        {
            Tour tour = new Tour(3, "Tour-1", "this is tour 1", "Graz", "Wien", "C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\Pictures\\Nyjisha.jpg", 350);
            TourLog tourLog = new TourLog(1, tour, "21.05.2021", "00:04:20", "Today was a wonderful day", 350, 3, 24, 30, 12, 10000, 350);
            Assert.IsNotNull(tourLog);
        }
        //[Test]
        //public void CountTourLogsForTour()
        //{
        //    Tour tour = new Tour(3, "Tour-1", "this is tour 1", "Graz", "Wien", "C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\Pictures\\Nyjisha.jpg", 350);
        //    TourLog tourLog1 = new TourLog(1, tour, "21.05.2021", "00:04:20", "Today was a wonderful day", 350, 3, 24, 30, 12, 10000, 350);
        //    TourLog tourLog2 = new TourLog(2, tour, "21.05.2021", "00:04:20", "I went up the stairs", 350, 3, 24, 30, 12, 10000, 350);
        //    TourLog tourLog3 = new TourLog(3, tour, "21.05.2021", "00:04:20", "I am exhausted", 350, 3, 24, 30, 12, 10000, 350);

        //    Assert.IsNotNull(tourLog);
        //}

        [Test]
        public void CheckDALFactoryFileSystemFalse()
        {
            DALFactoryMOCK dalf = new DALFactoryMOCK();
            Assert.IsFalse(dalf.useFileSystem);
        }
        [Test]
        public void CheckDALFactoryDatabaseTrue()
        {
            DALFactoryMOCK dalf = new DALFactoryMOCK();
            Assert.IsTrue(dalf.useFileSystem);
        }

        [Test]
        public void CheckImageCreationSuccess()
        {
            string routeInformation = NameGenerator.GenerateName(6);
            DAL.FileServer.FileAccess fa = new DAL.FileServer.FileAccess("C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\TestPictures\\");
            string path = fa.CreateImage("Graz", "Wien", routeInformation);
            Assert.IsNotNull(path);
        }
        [Test]
        public void CheckImageCreationFailed()
        {
            string routeInformation = NameGenerator.GenerateName(6);
            DAL.FileServer.FileAccess fa = new DAL.FileServer.FileAccess("C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\TestPictures\\");
            Assert.Throws<System.Net.WebException>(() => fa.CreateImage("","",routeInformation));

        }
        [Test]
        public void TourReportCreationSuccess()
        {
            Tour tour = new Tour(3, "Tour-1", "this is tour 1", "Graz", "Wien", "C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\Pictures\\Nyjisha.jpg", 350);
            string filePath = "TourReport_" + tour.Name + ".pdf";
            var document = new TourReportMOCK(tour);
            document.GeneratePdf(filePath);
            //Process.Start("explorer.exe", filePath);
            string filelocation = @"C:\Users\burak_y46me01\OneDrive\Desktop\TourPlannerrr\TourPlanner.Test\bin\Debug\netcoreapp3.1\TourReport_Tour-1.pdf";
            Assert.IsTrue(File.Exists(filelocation));
        }
        [Test]
        public void CreateNewTourFileSuccess()
        {
            DAL.FileServer.FileAccess fa = new DAL.FileServer.FileAccess("C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\TestPictures\\");
            fa.CreateNewTourFile("Tour-2", "das ist tour 2", "Graz", "Wien", "C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\Pictures\\Nyjisha.jpg", 350);
            Assert.IsTrue(File.Exists(filelocation));
        }
        [Test]
        public void CreateNewLogFileSuccess()
        {
            DAL.FileServer.FileAccess fa = new DAL.FileServer.FileAccess("C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\TestPictures\\");
            fa.CreateNewLogFile()
            Assert.IsTrue(File.Exists(filelocation));
        }
       


    }

}
      
