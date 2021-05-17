using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BL;
using TourPlanner.DAL.FileServer;
using TourPlanner.Models;

namespace TourPlanner.UI.ViewModels
{
    public class AddTourViewModel : BaseViewModel
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region instances
        private string name;
        private string from;
        private string to;
        private string description;
        private int distance;
        private ObservableCollection<Tour> tours;

        private ITourPlannerFactory tourFactory;

        private ICommand addCommand;
        public ICommand AddCommand => addCommand ??= new RelayCommand(AddTour);
        #endregion

        #region properties
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChangedEvent(nameof(name));
                }
            }
        }
        public string From
        {
            get
            {
                return from;
            }
            set
            {
                if (from != value)
                {
                    from = value;
                    RaisePropertyChangedEvent(nameof(from));
                }
            }
        }
        public string To
        {
            get
            {
                return to;
            }
            set
            {
                if (to != value)
                {
                    to = value;
                    RaisePropertyChangedEvent(nameof(to));
                }
            }
        }
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (description != value)
                {
                    description = value;
                    RaisePropertyChangedEvent(nameof(description));
                }
            }
        }

        public int Distance
        {
            get
            {
                return distance;
            }
            set
            {
                if (distance != value)
                {
                    distance = value;
                    RaisePropertyChangedEvent(nameof(distance));
                }
            }
        }

        public ObservableCollection<Tour> Tours { get => tours; set => tours = value; }



        #endregion
        #region constructor
        public AddTourViewModel()
        {
            this.tourFactory = TourPlannerFactory.GetInstance();
            tours = new ObservableCollection<Tour>();
            //logs = new ObservableCollection<TourLog>();
            foreach (Tour item in this.tourFactory.GetTours())
            {
                tours.Add(item);
            }
        }
        #endregion
        #region methods
        private void AddTour(object commandParameter)
        {
            try
            {
                string routeInformation = NameGenerator.GenerateName(6);
                FileAccess fa = new FileAccess("C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\Pictures\\");
                string path = fa.CreateImage(From, To, routeInformation);
                Tour addedTour = tourFactory.CreateTour(Name, Description, From, To, path, Distance);
                tours.Add(addedTour);
                log.Info("Tour " + Name + " succesfully added into TourList");

            }
            catch (Exception ex )
            {

                log.Error("Could not add tour " + name + " " + ex.Message);
            }
          
            
        }
        #endregion
    }
}
