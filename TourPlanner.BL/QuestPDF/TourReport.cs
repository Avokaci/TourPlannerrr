﻿using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.BL.QuestPDF
{
    /// <summary>
    /// Class to generate a pdf file of an existing tour with its corresponding tour information, image and tour logs. 
    /// </summary>
    public class TourReport : IDocument
    {
        ITourPlannerFactory tourPlannerFactory;
        public Tour tour { get; }
        /// <summary>
        /// Constructor of class TourReport. 
        /// </summary>
        /// <param name="tour"></param>
        public TourReport(Tour tour)
        {
            this.tourPlannerFactory = TourPlannerFactory.GetInstance();
            this.tour = tour;
        }
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        /// <summary>
        /// method that composes the pdf file with the splitted page components. 
        /// </summary>
        /// <param name="container"></param>
        public void Compose(IContainer container)
        {
            container
             .PaddingHorizontal(50)
             .PaddingVertical(50)
             .Page(page =>
             {
                 page.Header().Element(ComposeHeader);  
                 page.Content().Element(ComposeTourLogs);
                 page.Footer().AlignCenter().PageNumber("Page {number}");
             });
        }
        /// <summary>
        /// method to compose the header
        /// </summary>
        /// <param name="container"></param>
        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeColumn().Stack(stack =>
                {
                    stack.Item().Text($"Tour #{tour.Id}", TextStyle.Default.Size(20));
                    stack.Item().Text($"Tour name: {tour.Name:d}");
                    stack.Item().Text($"Tour description: {tour.Description:d}");
                    stack.Item().Text($"From: {tour.From:d}");
                    stack.Item().Text($"To: {tour.To:d}");
                    stack.Item().Text($"Distance: {tour.Distance:d}");
                });

            });
        }

       
        /// <summary>
        /// method to compose the tour logs table and image
        /// </summary>
        /// <param name="container"></param>
        void ComposeTourLogs(IContainer container)
        {
            container.PaddingTop(10).Decoration(decoration =>
            {
                
                // header
                decoration.Header().BorderBottom(1).Padding(5).Row(row =>
                {
                    row.ConstantColumn(25).Text("#");
                    row.RelativeColumn().Text("Date");
                    row.RelativeColumn().Text("Duration");
                    row.RelativeColumn().Text("Report");
                    row.RelativeColumn().Text("km");
                    row.RelativeColumn().Text("Rating");
                    row.RelativeColumn().Text("Ø Speed");
                    row.RelativeColumn().Text("Max Speed");
                    row.RelativeColumn().Text("Min Speed");
                    row.RelativeColumn().Text("Ø Steps");
                    row.RelativeColumn().Text("Burnt cal.");
                });

                // content
                decoration
                    .Content()
                    .Stack(stack =>
                    {
                        foreach (TourLog item in tourPlannerFactory.GetLogs(tour))
                        {
                            stack.Item().BorderBottom(1).BorderColor("CCC").Padding(5).Row(row =>
                            {
                                row.ConstantColumn(25).Text(item.Id);
                                row.RelativeColumn().Text(item.Date);
                                row.RelativeColumn().Text(item.TotalTime);
                                row.RelativeColumn().Text(item.Report);
                                row.RelativeColumn().Text(item.Distance);
                                row.RelativeColumn().Text(item.Rating);
                                row.RelativeColumn().Text(item.AverageSpeed);
                                row.RelativeColumn().Text(item.MaxSpeed);
                                row.RelativeColumn().Text(item.MinSpeed);
                                row.RelativeColumn().Text(item.AverageStepCount);
                                row.RelativeColumn().Text(item.BurntCalories);
                            });
                        }
                    });
                //map image
                decoration.Footer()
                 .Stack(stack =>
                 {
                     string imagePath = tour.RouteInformation;
                     imagePath.Replace(@"\\", @"\");
                     byte[] image = File.ReadAllBytes(imagePath);
                     stack.Item().PaddingBottom(5).Image(image);
                 });
            });
           
        }
       

        
    }

}

