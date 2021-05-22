using NUnit.Framework;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
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
            DAL.FileServer.FileAccess fa = new DAL.FileServer.FileAccess("C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\TestData\\");
            fa.CreateNewTourFile("Tour-2", "das ist tour 2", "Graz", "Wien", "C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\Pictures\\Nyjisha.jpg", 350);
            string[] files = System.IO.Directory.GetFiles("C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\TestData\\", "*_TourItem.txt", System.IO.SearchOption.TopDirectoryOnly);
            bool filexists = false;
            if (files.Length > 0)
            {
                filexists = true;
            }
            Assert.IsTrue(filexists);
        }
        [Test]
        public void CreateNewLogFileSuccess()
        {
            DAL.FileServer.FileAccess fa = new DAL.FileServer.FileAccess("C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\TestData\\");
            Tour tour = new Tour(3, "Tour-1", "this is tour 1", "Graz", "Wien", "C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\Pictures\\Nyjisha.jpg", 350);
            fa.CreateNewLogFile(tour, "21.05.2021", "00:04:20", "Today was a wonderful day", 350, 3, 24, 30, 12, 10000, 350);
            string[] files = System.IO.Directory.GetFiles("C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\TestData\\", "*_TourLog.txt", System.IO.SearchOption.TopDirectoryOnly);
            bool filexists = false;
            if (files.Length > 0)
            {
                filexists = true;
            }
            Assert.IsTrue(filexists);
        }
        [Test]
        public void CheckNameGeneratorOneUpercaseRestLowercaseSuccess()
        {
            NameGeneratorMOCK ng = new NameGeneratorMOCK();
            string res = ng.GenerateName(6);
  
            StringAssert.IsMatch(@"^[A-Z][a-z]{5}", res);
        }
        [Test]
        public void CheckNameGeneratorOnlyLowercaseFailure()
        {
            NameGeneratorMOCK ng = new NameGeneratorMOCK();
            string res = ng.GenerateName(6);

            StringAssert.DoesNotMatch(@"^[a-z]{6}", res);
        }
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
        public void ExportObjectInitializationSuccess()
        {
            Tour tour = new Tour(3, "Tour-1", "this is tour 1", "Graz", "Wien", "C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\Pictures\\Nyjisha.jpg", 350);
            TourLog tourLog1 = new TourLog(1, tour, "21.05.2021", "00:04:20", "Today was a wonderful day", 350, 3, 24, 30, 12, 10000, 350);
            TourLog tourLog2 = new TourLog(2, tour, "21.05.2021", "00:06:20", "it rained afterwards", 350, 3, 24, 30, 12, 10000, 350);
            TourLog tourLog3 = new TourLog(1, tour, "21.05.2021", "00:08:20", "i fought a goat", 350, 3, 24, 30, 12, 10000, 350);
            List<TourLog> logs = new List<TourLog>() { tourLog1, tourLog2, tourLog3 };
            ExportObject eo = new ExportObject(tour, logs);
            Assert.IsNotNull(eo);
        }
        [Test]
        public void CheckExportSuccess()
        {
            Tour tour = new Tour(3, "Tour-1", "this is tour 1", "Graz", "Wien", "C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\Pictures\\Nyjisha.jpg", 350);
            ITourPlannerFactory tourPlannerFactory = TourPlannerFactory.GetInstance();
            tourPlannerFactory.Export(tour);
            string filePath = @"C:\Users\burak_y46me01\OneDrive\Desktop\TourPlannerrr\ExportedObjects\Tour-1.json";
            Assert.IsTrue(File.Exists(filePath));
        }
        [Test]
        public void CheckExportFileCanNotBeCreatedBadNameFailure()
        {
            Tour tour = new Tour(3, "Tour-1|", "this is tour 1", "Graz", "Wien", "C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\Pictures\\Nyjisha.jpg", 350);
            ITourPlannerFactory tourPlannerFactory = TourPlannerFactory.GetInstance();
            Assert.Throws<System.IO.IOException>(() => tourPlannerFactory.Export(tour));
        }
        [Test]
        public void CheckExportWithLogsSuccess()
        {
            Tour tour = new Tour(3, "Tour-50", "this is tour 50", "Graz", "Wien", "C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\Pictures\\Nyjisha.jpg", 350);
            ITourPlannerFactory tourPlannerFactory = TourPlannerFactory.GetInstance();
            tourPlannerFactory.ExportWithLogs(tour);
            string filePath = @"C:\Users\burak_y46me01\OneDrive\Desktop\TourPlannerrr\ExportedObjects\Tour-50.json";
            Assert.IsTrue(File.Exists(filePath));
        }
        [Test]
        public void CheckImportSuccess()
        {
            ITourPlannerFactory tourPlannerFactory = TourPlannerFactory.GetInstance();
            tourPlannerFactory.Import(@"C:\Users\burak_y46me01\OneDrive\Desktop\TourPlannerrr\ExportedObjects\Tour-31.json");
            
        }
        [Test]
        public void CheckImportFileDoesNotExistFailure()
        {
            ITourPlannerFactory tourPlannerFactory = TourPlannerFactory.GetInstance();
            Assert.Throws<System.IO.FileNotFoundException>(() => tourPlannerFactory.Import(@"C:\Users\burak_y46me01\OneDrive\Desktop\TourPlannerrr\ExportedObjects\Tour-1000.json"));  
        }
        [Test]
        public void CheckImportWithLogsSuccess()
        {
            ITourPlannerFactory tourPlannerFactory = TourPlannerFactory.GetInstance();
            tourPlannerFactory.ImportWithLogs(@"C:\Users\burak_y46me01\OneDrive\Desktop\TourPlannerrr\ExportedObjects\Tour-31.json");
        }
       
        [Test]
        public void y()
        {
         
        }
        [Test]
        public void z()
        {
          
        }
    }

}
      
