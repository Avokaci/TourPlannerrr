using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Linq;
using TourPlanner.UI.Views;
using TourPlanner.Models;
using TourPlanner.BL;
using TourPlanner.DAL.FileServer;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TourPlanner.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Instances
        private ObservableCollection<Tour> tours;
        private ObservableCollection<TourLog> logs;
        private Tour currentItem;
        private ImageSource currentItemImageSource;
        private string searchCommand;

        private ICommand randomGenerateItemCommand;
        private ICommand randomGenerateLogCommand;
        private ICommand popUpAdd;

        private ITourPlannerFactory tourPlannerFactory;
        public ICommand PopUpAdd => popUpAdd ??= new RelayCommand(OpenAddTourWindow);
        public ICommand RandomGenerateItemCommand => randomGenerateItemCommand ??= new RelayCommand(RandomGenerateItem);
        public ICommand RandomGenerateLogCommand => randomGenerateLogCommand ??= new RelayCommand(RandomGenerateLog);

        private void RandomGenerateItem(object commandParameter)
        {
            //rand Stuff
            Array values = Enum.GetValues(typeof(Cities));
            Random random = new Random();
            Cities randomCity1 = (Cities)values.GetValue(random.Next(values.Length));
            Cities randomCity2 = (Cities)values.GetValue(random.Next(values.Length));
            int distance = random.Next(1, 700);
            string routeInformation = NameGenerator.GenerateName(6);

            //generate image
            FileAccess fa = new FileAccess("C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\Pictures\\");
            string path = fa.CreateImage(randomCity1.ToString(), randomCity2.ToString(), routeInformation);

            //generate Tour item
            Tour generatedItem = tourPlannerFactory.CreateTour(NameGenerator.GenerateName(6), NameGenerator.GenerateName(15),
                randomCity1.ToString(), randomCity2.ToString(), path, distance);
            tours.Add(generatedItem);            
        }
        private void RandomGenerateLog(object commandParameter)
        {
            TourLog generatedLog = tourPlannerFactory.CreateTourLog(CurrentItem, NameGenerator.GenerateName(6), 
                NameGenerator.GenerateName(6), NameGenerator.GenerateName(6), 0, 0, 0, 0, 0, 0, 0);
        }

        #endregion

        #region Properties
        public IEnumerable<Tour> MyFilteredItems
        {
            get
            {
                if (searchCommand == null) return tours;

                return tours.Where(x => x.Name.ToUpper().StartsWith(searchCommand.ToUpper()));
            }
        }
        public ObservableCollection<Tour> Tours { get => tours; set => tours = value; }
        public ObservableCollection<TourLog> Logs { get => logs; set => logs = value; }
        public Tour CurrentItem
        {
            get
            {
                return currentItem;
            }
            set
            {
                if ((currentItem != value) && (value != null))
                {
                    currentItem = value;
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(currentItem.RouteInformation);
                    image.EndInit();
                    CurrentItemImageSource = image;
                    RaisePropertyChangedEvent(nameof(currentItem));
                    RaisePropertyChangedEvent(nameof(currentItemImageSource));
                }
            }
        }
        public ImageSource CurrentItemImageSource
        {
            get
            {
                return currentItemImageSource;
            }
            set
            {
                if (currentItemImageSource != value)
                {
                    currentItemImageSource = value;
                    RaisePropertyChangedEvent(nameof(currentItem));
                }
            }
        }
        public string SearchCommand
        {
            get
            {
                return searchCommand;
            }
            set
            {
                if (searchCommand != value)
                {
                    searchCommand = value;
                    RaisePropertyChangedEvent("SearchText");
                    RaisePropertyChangedEvent("MyFilteredItems");
                }
            }
        }
      
        #endregion

        #region Constructor
        public MainViewModel()
        {
            this.tourPlannerFactory = TourPlannerFactory.GetInstance();
            tours = new ObservableCollection<Tour>();
            logs = new ObservableCollection<TourLog>();
            foreach (Tour item in this.tourPlannerFactory.GetTours())
            {
                tours.Add(item);
            }

        }
        #endregion

        #region Methods
        private void OpenAddTourWindow(object commandParameter)
        {
            AddTourWindow atw = new AddTourWindow();
            atw.DataContext = new AddTourViewModel();
            atw.ShowDialog();
        }
        #endregion

    }
}
