using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace TODOList
{
    internal class MainViewModel:BaseViewModel
    {
        #region Commands
        public ICommand Open { get; set; }
        public ICommand MarkAsCompleted { get; set; }
        public ICommand Remove { get; set; }
        public ICommand FinishTask { get; set; }
        public ICommand CloseApp { get; set; }
        public ICommand Minimize { get; set; }
        public ICommand Confirm { get; set; }

        public ICommand WindowClosing { get; set; }
        public ICommand SyncChange { get; set; }
        public ICommand OpenInfo { get; set; }
        public ICommand AddUser { get; set; }
        public ICommand RemoveUser { get; set; }
        public ICommand DateRange { get; set; }
        #endregion

        #region Fields and Properties
        GoogleCalendarService calendarService;

        public string XmlFilePath = "TasksSerialization.xml";

        private int index;
        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                index = Main.IndexOf(tasks[value]);
                OnPropertyChange("Index");
            }
        }

        private TaskViewModel SelectedItem;
        public TaskViewModel Selected
        {
            get
            {
                return SelectedItem;
            }
            set
            {
                SelectedItem = value;
                OnPropertyChange("Selected");
            }
        }
        private ObservableCollection<TaskViewModel> tasks;
        public ObservableCollection<TaskViewModel> Tasks
        {
            get { return tasks ?? (tasks = new ObservableCollection<TaskViewModel>()); }
            set { tasks = value; OnPropertyChange("Tasks"); }
        }

        //Variable to handle main collection when user select range of data
        private ObservableCollection<TaskViewModel> main;
        public ObservableCollection<TaskViewModel> Main
        {
            get { return main ?? (main = new ObservableCollection<TaskViewModel>()); }
            set { main = value; OnPropertyChange("Temp"); }
        }
        #endregion

        #region Constructor and Instance
        public static MainViewModel Instance = new MainViewModel();

        /// <summary>
        /// Creates new instance of MainViewModel
        /// </summary>
        public MainViewModel()
        {
            Selected = null;
            Confirm = new RelayCommand<object[]>(NewTask);
            Open = new RelayCommand<string>(OpenWindow);
            FinishTask = new NormalCommand(_FinishTask);
            CloseApp = new RelayCommand<Window>(Close);
            Minimize = new RelayCommand<Window>(minimize);
            WindowClosing = new NormalCommand(Closing);
            OpenInfo = new NormalCommand(openInfo);
            Remove = new NormalCommand(RemoveSelected);
            AddUser = new NormalCommand(AddParticipants);
            RemoveUser = new RelayCommand<Int32>(RemoveParticipants);
            SyncChange = new NormalCommand(SyncEvent);
            DateRange = new RelayCommand<string>(SetDateRange);

            calendarService = new GoogleCalendarService();

            if (File.Exists(XmlFilePath))
            {
                LoadFromXml();
            }
            else
            {
                Main = new ObservableCollection<TaskViewModel>();
            }

            SetDateRange("all");

            //RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            //registryKey.SetValue("TODOList", System.Reflection.Assembly.GetExecutingAssembly().Location);

        }


        #endregion

        #region Methods
        /// <summary>
        /// Select task within selected range
        /// </summary>
        /// <param name="range">Capabilities(All,this day,next day,this week,next week,this month,next month)</param>
        public void SetDateRange(string range)
        {
            Main = new ObservableCollection<TaskViewModel>(Main.OrderBy(i => i.StartDate).ToList());
            switch (range)
            {
                case "all":
                    Tasks = Main;
                    break;
                case "this day":
                    Tasks = new ObservableCollection<TaskViewModel>(Main.Where(i => i.StartDate.Day == DateTime.Now.Day).ToList());
                    break;
                case "next day":
                    Tasks = new ObservableCollection<TaskViewModel>(Main.Where(i => i.StartDate.Day == DateTime.Now.AddDays(1).Day).ToList());
                    break;
                case "this week":
                    Tasks = new ObservableCollection<TaskViewModel>(Main.Where(i => (i.StartDate >= DateTime.Now 
                    && i.StartDate <= DateTime.Now.AddDays(7))).ToList());
                    break;
                case "next week":
                    Tasks = new ObservableCollection<TaskViewModel>(Main.Where(i => (i.StartDate >= DateTime.Now.AddDays(7) 
                    && i.StartDate <= DateTime.Now.AddDays(14))).ToList());
                    break;
                case "this month":
                    Tasks = new ObservableCollection<TaskViewModel>(Main.Where(i => i.StartDate.Month == DateTime.Now.Month).ToList());
                    break;
                case "next month":
                    Tasks = new ObservableCollection<TaskViewModel>(Main.Where(i => i.StartDate.Month == DateTime.Now.AddMonths(1).Month).ToList());
                    break;
            }
        }
        /// <summary>
        /// Add participant to selected event
        /// </summary>
        private void AddParticipants()
        {
            AddParticipantsWindow participantsWindow = new AddParticipantsWindow(this.Index);
            participantsWindow.Show();
        }
        /// <summary>
        /// Remove participants from selected event
        /// </summary>
        /// <param name="index"></param>
        private void RemoveParticipants(int index)
        {
            if (index > -1) Main[Index].Attendees.RemoveAt(index);
            else MessageBox.Show("First choose mail to delete!", "Wrong index selected!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void SyncEvent()
        {
            calendarService.InsertIntoCalendar(Main[Index]);
            if (calendarService.CheckIfEventExists(Main[Index]))
            {
                MessageBox.Show("Event successful synced with Google Calendar!","Synced!",MessageBoxButton.OK, MessageBoxImage.Information);
                Main[Index].IsSynced = true;
            }
            else
            {
                MessageBox.Show("There was an error when syncing", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Remove selected task from list and if synced with google remove from google calendar
        /// </summary>
        private void RemoveSelected()
        {
            if (Index >= 0)
                if (Main[Index].IsSynced) calendarService.RemoveEvent(Main[Index]);
            Main.RemoveAt(Index);

            SetDateRange("all");
        }
        /// <summary>
        /// Open NewTask window to add task or subtask
        /// </summary>
        /// <param name="tag">Determines which one, task or subtask, want to add</param>
        private void OpenWindow(string tag)
        {
            NewTask AddNew = new NewTask();
            AddNew.Confirm.Tag = tag;
            if (tag == "Edit")
            {
                AddNew.TitleBox.Text = Main[Index].Title;
                AddNew.LocationBox.Text = Main[Index].Location;
                AddNew.DateSelector.SelectedDate = Main[Index].StartDate;
                AddNew.StartHours.Value = Main[Index].StartDate.Hour;
                AddNew.StartMinutes.Value = Main[Index].StartDate.Minute;
                AddNew.EndHours.Value = Main[Index].EndDate.Hour;
                AddNew.EndMinutes.Value = Main[Index].EndDate.Minute;
                AddNew.RepeatBox.IsChecked = Main[Index].IsRepeated;
                AddNew.IntervalSelector.SelectedItem = Main[Index].Interval;
                AddNew.Desc.Text = Main[Index].Description;
            }
            AddNew.ShowDialog();

            
        }
        /// <summary>
        /// Add new task or subtask
        /// </summary>
        /// <param name="param">Object array with parameters to add new items</param>
        public void NewTask(object[] param)
        {
            (param[0] as Window).Close();
            List<object> classParam=param.ToList();
            classParam.RemoveAt(0);
            if (classParam[4] == null) { classParam[4] = DateTime.Now.ToShortDateString(); classParam[5]=DateTime.Now.Hour + 1; }
            if (classParam[0].ToString() == String.Empty) classParam[0] = new StringBuilder("(Empty subject)");
            if (classParam[10].ToString() == "Edit") editTask(classParam);
            else Main.Add(new TaskViewModel(classParam));

            SetDateRange("all");
        }
        /// <summary>
        /// Finish and delete task from list
        /// </summary>
        private void _FinishTask()
        {
            if(Index>=0)
                Main[Index].IsCompleted=true;

            SetDateRange("all");
        }
        /// <summary>
        /// Set window to minimize state
        /// </summary>
        /// <param name="window">Determines which window minimize</param>
        private void minimize(Window window)
        {
            if (window != null) window.WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// Close window (if it is MainScreen window then close program)
        /// </summary>
        /// <param name="window">Determines which window close</param>
        private void Close(Window window)
        {
            if (window != null) window.Close();
            if (window.Name == "MainScreen") Environment.Exit(1);
        }
        /// <summary>
        /// Save tasks to xml file while closing program
        /// </summary>
        private void Closing()
        {
            SaveToXml();
        }
        /// <summary>
        /// Method to open InfoWindow
        /// </summary>
        private void openInfo()
        {
            InfoWindow info = new InfoWindow();
            info.ShowDialog();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        private void editTask(List<object> Prop)
        {
            Main[Index].Title = Prop[0].ToString();
            Main[Index].Location = Prop[1].ToString();
            Main[Index].IsRepeated = (bool)Prop[2];
            Main[Index].Interval = ((ComboBoxItem)Prop[3]).Content.ToString();

            Main[Index].StartDate = Convert.ToDateTime(Prop[4]);
            Main[Index].StartDate = Main[Index].StartDate.AddHours(Convert.ToInt32(Prop[5]));
            Main[Index].StartDate = Main[Index].StartDate.AddMinutes(Convert.ToInt32(Prop[6]));

            Main[Index].EndDate = Convert.ToDateTime(Prop[4]);
            Main[Index].EndDate = Main[Index].EndDate.AddHours(Convert.ToInt32(Prop[7]));
            Main[Index].EndDate = Main[Index].EndDate.AddMinutes(Convert.ToInt32(Prop[8]));

            if (Main[Index].StartDate > Main[Index].EndDate) Main[Index].EndDate = Main[Index].StartDate.AddHours(1);

            Main[Index].SetNextNotifyDate();
            if (Main[Index].IsSynced) calendarService.EditEvent(Main[Index]);

            SetDateRange("all");
        }


        #endregion
        #region Serialization
        /// <summary>
        /// Serialize tasks
        /// </summary>
        public void SaveToXml()
        {
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(ObservableCollection<TaskViewModel>));

            var path = XmlFilePath;
            FileStream file = System.IO.File.Create(path);
            writer.Serialize(file, main);
            file.Close();
        }
        /// <summary>
        /// Deserialize tasks
        /// </summary>
        public void LoadFromXml()
        {
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(ObservableCollection<TaskViewModel>));
            System.IO.StreamReader file = new StreamReader(XmlFilePath);
            main = reader.Deserialize(file) as ObservableCollection<TaskViewModel>;
            file.Close();
        }
        #endregion
    }
}
