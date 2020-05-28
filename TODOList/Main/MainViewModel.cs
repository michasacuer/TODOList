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
                index = value;
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

            calendarService = new GoogleCalendarService();

            if (File.Exists(XmlFilePath))
            {
                LoadFromXml();
            }
            else
            {
                Tasks = new ObservableCollection<TaskViewModel>();
            }

            //RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            //registryKey.SetValue("TODOList", System.Reflection.Assembly.GetExecutingAssembly().Location);

        }
        #endregion

        #region Methods
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
            if (index > -1) Tasks[Index].Attendees.RemoveAt(index);
            else MessageBox.Show("First choose mail to delete!", "Wrong index selected!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void SyncEvent()
        {
            calendarService.InsertIntoCalendar(tasks[Index]);
            if (calendarService.CheckIfEventExists(tasks[Index]))
            {
                MessageBox.Show("Event successful synced with Google Calendar!","Synced!",MessageBoxButton.OK, MessageBoxImage.Information);
                tasks[Index].IsSynced = true;
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
                if (Tasks[Index].IsSynced) calendarService.RemoveEvent(tasks[Index]);
                Tasks.RemoveAt(Index);
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
                AddNew.TitleBox.Text = tasks[Index].Title;
                AddNew.LocationBox.Text = tasks[Index].Location;
                AddNew.DateSelector.SelectedDate = tasks[Index].StartDate;
                AddNew.StartHours.Value = tasks[Index].StartDate.Hour;
                AddNew.StartMinutes.Value = tasks[Index].StartDate.Minute;
                AddNew.EndHours.Value = tasks[Index].EndDate.Hour;
                AddNew.EndMinutes.Value = tasks[Index].EndDate.Minute;
                AddNew.RepeatBox.IsChecked = tasks[Index].IsRepeated;
                AddNew.IntervalSelector.SelectedItem = tasks[Index].Interval;
                AddNew.Desc.Text = tasks[Index].Description;
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
            if (classParam[4] == null) classParam[4] = DateTime.Now.ToShortDateString();
            if (classParam[0].ToString() == String.Empty) classParam[0] = new StringBuilder("(Empty subject)");
            if (classParam[10].ToString() == "Edit") editTask(classParam);
            else Tasks.Add(new TaskViewModel(classParam));
        }
        /// <summary>
        /// Finish and delete task from list
        /// </summary>
        private void _FinishTask()
        {
            if(Index>=0)
                Tasks[Index].IsCompleted=true;
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
            tasks[Index].Title = Prop[0].ToString();
            tasks[Index].Location = Prop[1].ToString();
            tasks[Index].IsRepeated = (bool)Prop[2];
            tasks[Index].Interval = ((ComboBoxItem)Prop[3]).Content.ToString();

            tasks[Index].StartDate = Convert.ToDateTime(Prop[4]);
            tasks[Index].StartDate = tasks[Index].StartDate.AddHours(Convert.ToInt32(Prop[5]));
            tasks[Index].StartDate = tasks[Index].StartDate.AddMinutes(Convert.ToInt32(Prop[6]));

            tasks[Index].EndDate = Convert.ToDateTime(Prop[4]);
            tasks[Index].EndDate = tasks[Index].EndDate.AddHours(Convert.ToInt32(Prop[7]));
            tasks[Index].EndDate = tasks[Index].EndDate.AddMinutes(Convert.ToInt32(Prop[8]));

            if (tasks[Index].StartDate > tasks[Index].EndDate) tasks[Index].EndDate = tasks[Index].StartDate.AddHours(1);

            tasks[Index].SetNextNotifyDate();
            if (tasks[Index].IsSynced) calendarService.EditEvent(tasks[Index]);
        }
        /// <summary>
        /// Serialize tasks
        /// </summary>
        private void SaveToXml()
        {
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(ObservableCollection<TaskViewModel>));

            var path = XmlFilePath;
            FileStream file = System.IO.File.Create(path);
            writer.Serialize(file, Tasks);
            file.Close();
        }
        /// <summary>
        /// Deserialize tasks
        /// </summary>
        private void LoadFromXml()
        {
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(ObservableCollection<TaskViewModel>));
            System.IO.StreamReader file = new StreamReader(XmlFilePath);
            Tasks = reader.Deserialize(file) as ObservableCollection<TaskViewModel>;
            file.Close();
        }
        #endregion
    }
}
