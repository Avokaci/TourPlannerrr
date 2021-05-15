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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region Instances
        private ObservableCollection<Tour> tours;
        private ObservableCollection<TourLog> logs;
        private Tour currentItem;
        private TourLog currentLog;
        private ImageSource currentItemImageSource;
        private string searchCommand;

        
        private ICommand popUpAddTour;
        private ICommand popUpChangeTour;
        private ICommand popUpAddLog;
        private ICommand popUpChangeLog;
        private ICommand randomGenerateItemCommand;
        private ICommand randomGenerateLogCommand;
        private ICommand importCommand;

        private ITourPlannerFactory tourPlannerFactory;
        public ICommand PopUpAddTour => popUpAddTour ??= new RelayCommand(OpenAddTourWindow);
        public ICommand PopUpChangeTour => popUpChangeTour ??= new RelayCommand(OpenChangeTourWindow);
        public ICommand PopUpAddLog => popUpAddLog ??= new RelayCommand(OpenAddLogWindow);
        public ICommand PopUpChangeLog => popUpChangeLog ??= new RelayCommand(OpenChangeLogWindow);
        public ICommand ImportCommand => importCommand ??= new RelayCommand(Import);


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
            string name = NameGenerator.GenerateName(6);
            //generate image
            FileAccess fa = new FileAccess("C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\Pictures\\");
            string path = fa.CreateImage(randomCity1.ToString(), randomCity2.ToString(), routeInformation);
            log.Debug("Image for random Tour generated");

            //generate Tour item
            Tour generatedItem = tourPlannerFactory.CreateTour(name, NameGenerator.GenerateName(15),
                randomCity1.ToString(), randomCity2.ToString(), path, distance);
            tours.Add(generatedItem);
            log.Debug("Random tour " + name + " succesfully generated and added to list");


        }
        private void RandomGenerateLog(object commandParameter)
        {
            Random random = new Random();
           

            TourLog generatedLog = tourPlannerFactory.CreateTourLog(CurrentItem, DateTime.Now.ToString(), 
                "00:04:02", NameGenerator.GenerateName(20), random.Next(1, 700), random.Next(1, 5), 
                random.Next(5, 30), random.Next(1, 30), random.Next(1, 30), random.Next(500, 1000), random.Next(1, 500));
            //Logs.Add(generatedLog);
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
                    logs.Clear();
                    FillDataGrid(currentItem);

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

        public TourLog CurrentLog 
        {
            get
            {
                return currentLog;
            }
            set
            {
                if ((currentLog != value) && (value != null))
                {
                    currentLog = value;
                    RaisePropertyChangedEvent(nameof(CurrentLog));
                }
            }
        }

        #endregion

        #region Constructor
        public MainViewModel()
        {
            this.tourPlannerFactory = TourPlannerFactory.GetInstance();
            InitListBox();
            InitDataGrid();
        }

        private void InitListBox()
        {
            tours = new ObservableCollection<Tour>();
            FillListBox();
        }

        private void FillListBox()
        {
            foreach (Tour item in this.tourPlannerFactory.GetTours())
            {
                tours.Add(item);
            }
        }

        private void InitDataGrid()
        {
            logs = new ObservableCollection<TourLog>();
        }

        private void FillDataGrid(Tour curItem)
        {
            foreach (TourLog item in this.tourPlannerFactory.GetLogs(curItem))
            {
                logs.Add(item);
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
        private void OpenChangeTourWindow(object commandParameter)
        {
            ChangeTourWindow atw = new ChangeTourWindow();
            atw.DataContext = new ChangeTourViewModel();
            atw.ShowDialog();
        }
        private void OpenAddLogWindow(object commandParameter)
        {
            AddLogWindow atw = new AddLogWindow();
            atw.DataContext = new AddLogViewModel();
            atw.ShowDialog();
        }
        private void OpenChangeLogWindow(object commandParameter)
        {
            ChangeLogWindow atw = new ChangeLogWindow();
            atw.DataContext = new ChangeLogViewModel();
            atw.ShowDialog();
        }
        private void Import(object commandParameter)
        {
            string filePath;
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = openFileDlg.ShowDialog();

            if (result == true)
            {
                filePath = openFileDlg.FileName;
                tourPlannerFactory.Import(filePath);
            }
            tours.Clear();
            FillListBox();
        }
        #endregion

    }
}
