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
using TourPlanner.BL.QuestPDF;
using System.Diagnostics;
using QuestPDF.Fluent;
using System.Windows;

namespace TourPlanner.UI.ViewModels
{
    /// <summary>
    /// MainViewModel class which serves as the ViewModel for the MainWindow. It allows for the creation, deletion and modification of tours and logs for the tours. 
    /// Furthermore it allows for import and export of tours and logs from and into a file. And lets the user generate a report of a tour with its corresponding logs. 
    /// </summary>
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
        private ICommand exportCommand;

        private ICommand generateReportCommand;

        private ITourPlannerFactory tourPlannerFactory;
        public ICommand PopUpAddTour => popUpAddTour ??= new RelayCommand(OpenAddTourWindow);
        public ICommand PopUpChangeTour => popUpChangeTour ??= new RelayCommand(OpenChangeTourWindow);
        public ICommand PopUpAddLog => popUpAddLog ??= new RelayCommand(OpenAddLogWindow);
        public ICommand PopUpChangeLog => popUpChangeLog ??= new RelayCommand(OpenChangeLogWindow);
        public ICommand ImportCommand => importCommand ??= new RelayCommand(Import);
        public ICommand ExportCommand => exportCommand ??= new RelayCommand(Export);

        public ICommand GenerateReportCommand => generateReportCommand ??= new RelayCommand(GenerateReport);


        public ICommand RandomGenerateItemCommand => randomGenerateItemCommand ??= new RelayCommand(RandomGenerateItem);
        public ICommand RandomGenerateLogCommand => randomGenerateLogCommand ??= new RelayCommand(RandomGenerateLog);

        public AddLogViewModel addLogViewModel;
        public ChangeLogViewModel changeLogViewModel;
        public AddTourViewModel addTourViewModel;
        public ChangeTourViewModel changeTourViewModel;


        /// <summary>
        /// Method to generate a random Tour item, via button press on the MainWindow.
        /// </summary>
        /// <param name="commandParameter"></param>
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
            string path ="";
            try
            {
                //generate image
                FileAccess fa = new FileAccess("C:\\Users\\burak_y46me01\\OneDrive\\Desktop\\TourPlannerrr\\Pictures\\");
                path = fa.CreateImage(randomCity1.ToString(), randomCity2.ToString(), routeInformation);
                log.Info("Image for random Tour generated");
            }
            catch (Exception ex)
            {

                log.Error("Could not create Image for random Tour: " + ex.Message);
            }

            try
            {
                //generate Tour item
                Tour generatedItem = tourPlannerFactory.CreateTour(name, NameGenerator.GenerateName(15),
                    randomCity1.ToString(), randomCity2.ToString(), path, distance);
                tours.Add(generatedItem);
                log.Info("Random tour " + name + " succesfully generated and added to list");
            }
            catch (Exception ex)
            {
                log.Error("Could not create random Tour: " + ex.Message);
            }
        }
        /// <summary>
        /// Method to generate a random Tour Log , via button press on the MainWindow.
        /// </summary>
        /// <param name="commandParameter"></param>
        private void RandomGenerateLog(object commandParameter)
        {
            Random random = new Random();

            try
            {
                TourLog generatedLog = tourPlannerFactory.CreateTourLog(CurrentItem,
                                                                        DateTime.Now.ToString(),
                                                                        "00:04:02",
                                                                        NameGenerator.GenerateName(20),
                                                                        random.Next(1, 700),
                                                                        random.Next(1, 5),
                                                                        random.Next(5, 30),
                                                                        random.Next(1, 30),
                                                                        random.Next(1, 30),
                                                                        random.Next(500, 1000),
                                                                        random.Next(1, 500));
                log.Info("Random log for Tour " + currentItem.Name + " with id " + currentItem.Id + " generated");

                //Logs.Add(generatedLog);
            }
            catch (Exception ex)
            {

                log.Error("Could not create Random log for Tour " + currentItem.Name + " with id " + currentItem.Id + " " + ex.Message);
            }
         
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
        /// <summary>
        /// Constructor for MainViewModel class that accesses Singleton instance of the Items. Also initalizes Listbox and Datagrid in MainWindow and fills them.
        /// </summary>
        public MainViewModel()
        {
            this.tourPlannerFactory = TourPlannerFactory.GetInstance();
            InitListBox();
            InitDataGrid();
        }

        /// <summary>
        /// Method that initializes the List Box 
        /// </summary>
        private void InitListBox()
        {
            tours = new ObservableCollection<Tour>();
            FillListBox();
        }
        /// <summary>
        /// Method that fills the listbox with the corresponding tour data.
        /// </summary>
        private void FillListBox()
        {
            foreach (Tour item in this.tourPlannerFactory.GetTours())
            {
                tours.Add(item);
            }
        }
        /// <summary>
        /// Method that initializes the Data Grid
        /// </summary>
        private void InitDataGrid()
        {
            logs = new ObservableCollection<TourLog>();
        }
        /// <summary>
        /// Method that fills the data grid with the corresponding tour log data.
        /// </summary>
        private void FillDataGrid(Tour curItem)
        {
            foreach (TourLog item in this.tourPlannerFactory.GetLogs(curItem))
            {
                logs.Add(item);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method that opens a new window to add tours. 
        /// </summary>
        /// <param name="commandParameter"></param>
        private void OpenAddTourWindow(object commandParameter)
        {
            this.addTourViewModel = new AddTourViewModel();
            AddTourWindow atw = new AddTourWindow();
            atw.DataContext = this.addTourViewModel;
            atw.ShowDialog();
            tours.Clear();
            FillListBox();
        }
        /// <summary>
        /// Method that opens a new window to change an existing tour. Gets the selected tour in the listbox as a parameter 
        /// </summary>
        /// <param name="commandParameter"></param>
        private void OpenChangeTourWindow(object commandParameter)
        {
            this.changeTourViewModel = new ChangeTourViewModel();
            changeTourViewModel.CurrentItem = CurrentItem;
            ChangeTourWindow atw = new ChangeTourWindow();
            atw.DataContext = this.changeTourViewModel;
            atw.ShowDialog();
            tours.Clear();
            FillListBox();
        }
        /// <summary>
        /// Method that opens a new window to add a log to an existing tour. Gets the selected tour in the listbox as a parameter 
        /// </summary>
        /// <param name="commandParameter"></param>
        private void OpenAddLogWindow(object commandParameter)
        {
           
            this.addLogViewModel = new AddLogViewModel();
            addLogViewModel.CurrentTour = CurrentItem;
            AddLogWindow view = new AddLogWindow();
            view.DataContext = this.addLogViewModel;
            view.ShowDialog();
            logs.Clear();
            FillDataGrid(CurrentItem);
         
        }
         /// <summary>
        /// Method that opens a new window to change an existing log. Gets the selected Log in the listbox as a parameter 
        /// </summary>
        /// <param name="commandParameter"></param>
        private void OpenChangeLogWindow(object commandParameter)
        {
            this.changeLogViewModel = new ChangeLogViewModel();
            //changeLogViewModel.CurrentItem = CurrentItem;
            ChangeLogWindow atw = new ChangeLogWindow();
            atw.DataContext = this.changeLogViewModel;
            atw.ShowDialog();
            logs.Clear();
            FillListBox();
        }
        /// <summary>
        /// Method to exports tours and their corresponding tour logs from a file. 
        /// </summary>
        /// <param name="commandParameter"></param>
        private void Export(object commandParameter)
        {
            try
            {
                tourPlannerFactory.Export(CurrentItem);
                MessageBox.Show("Export success!");
            }
            catch (Exception ex)
            {

                log.Error("Could not export tour " + ex.Message);
            }
        }
        /// <summary>
        /// Method to import tours and their corresponding tour logs from a file. 
        /// </summary>
        /// <param name="commandParameter"></param>
        private void Import(object commandParameter)
        {
            string filePath;
            try
            {
                Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

                Nullable<bool> result = openFileDlg.ShowDialog();

                if (result == true)
                {
                    filePath = openFileDlg.FileName;
                    tourPlannerFactory.Import(filePath);
                }
                tours.Clear();
                FillListBox();
                MessageBox.Show("Import success!");

            }
            catch (Exception ex)
            {

                log.Error("Could not import from filepath " + ex.Message);
            }
           
        }
        /// <summary>
        /// Method to generate a pdf report of an existing tour with its image and tour logs. 
        /// </summary>
        /// <param name="commandParameter"></param>
        public void GenerateReport(object commandParameter)
        {
            try
            {
                string filePath = "TourReport_" + currentItem.Name + ".pdf";
                var document = new TourReport(CurrentItem);
                document.GeneratePdf(filePath);

                Process.Start("explorer.exe", filePath);
            }
            catch (Exception ex)
            {

                log.Error("Could not generate Tour Report " + ex.Message);
            }

        }
        #endregion

    }
}
