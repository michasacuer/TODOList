using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Timers;
using Microsoft.Win32;
using TODOList.Properties;
using System.Reflection;
using IWshRuntimeLibrary;

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
        public ICommand Exit { get; set; }
        public ICommand Show { get; set; }
        public ICommand Startup { get; set; }
        public ICommand WindowLoad { get; set; }
        #endregion

        #region Fields and Properties
        private Timer myTimer;

        public string StartupGraphics
        {
            get
            {
                if (Settings.Default.IfStartup) return "Graphics/color_os.png";
                else return "Graphics/white_os.png";
            }
        }

        private int index;
        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                if (value>-1) index = MainCollection.Main.IndexOf(Tasks[value]);
                else index = -1;
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
            DateRange = new RelayCommand<string>(SetDateRange);
            Exit = new NormalCommand(ExitApp);
            Show = new RelayCommand<Window>(ShowApp);
            Startup = new NormalCommand(SetStartup);
            WindowLoad = new NormalCommand(LoadWindow);

            LoadWindow();

            SetDateRange("all");

            CheckToday();
            if (DateTime.Now.Minute == 30 || DateTime.Now.Minute == 0) CheckTasks(true);
            else { CheckTasks(false); SetTimer(30 - (DateTime.Now.Minute % 30)); }

        }


        #endregion

        #region Methods
        /// <summary>
        /// Load list while window loaded
        /// </summary>
        private void LoadWindow()
        {
            if (System.IO.File.Exists(Serialization.path))
            {
                MainCollection.Main = Serialization.LoadFromXml();
            }
            else
            {
                MainCollection.Main = new ObservableCollection<TaskViewModel>();
            }
        }
        /// <summary>
        /// Close app from tray
        /// </summary>
        private void ExitApp()
        {
            Application.Current.MainWindow.Close();
        }
        /// <summary>
        /// Unhide app from tray
        /// </summary>
        /// <param name="window"></param>
        private void ShowApp(Window window)
        {
            if (window != null) window.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Select task within selected range
        /// </summary>
        /// <param name="range">Capabilities(All,this day,next day,this week,next week,this month,next month)</param>
        public void SetDateRange(string range)
        {
            Tasks = SelectDateRange.SelectTaskFromRange(MainCollection.Main, range);
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
            if (index > -1) AttendeeManagment.Remove(MainCollection.Main[Index].Attendees,index);
            else MessageBox.Show("First choose mail to delete!", "Wrong index selected!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void SyncEvent()
        {
            GoogleCalendarService.InsertIntoCalendar(MainCollection.Main[Index]);
            if (GoogleCalendarService.CheckIfEventExists(MainCollection.Main[Index]))
            {
                MessageBox.Show("Event successful synced with Google Calendar!","Synced!",MessageBoxButton.OK, MessageBoxImage.Information);
                MainCollection.Main[Index].IsSynced = true;
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
            MainCollection.RemoveSelected(Index);

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
                AddNew.TitleBox.Text = MainCollection.Main[Index].Title;
                AddNew.LocationBox.Text = MainCollection.Main[Index].Location;
                AddNew.DateSelector.SelectedDate = MainCollection.Main[Index].StartDate;
                AddNew.StartHours.Value = MainCollection.Main[Index].StartDate.Hour;
                AddNew.StartMinutes.Value = MainCollection.Main[Index].StartDate.Minute;
                AddNew.EndHours.Value = MainCollection.Main[Index].EndDate.Hour;
                AddNew.EndMinutes.Value = MainCollection.Main[Index].EndDate.Minute;
                AddNew.RepeatBox.IsChecked = MainCollection.Main[Index].IsRepeated;
                AddNew.IntervalSelector.SelectedItem = MainCollection.Main[Index].Interval;
                AddNew.Desc.Text = MainCollection.Main[Index].Description;
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
            else
            {
                DateTime start = Convert.ToDateTime(classParam[4]);
                start = start.AddHours(Convert.ToInt32(classParam[5]));
                start = start.AddMinutes(Convert.ToInt32(classParam[6]));
                DateTime end = Convert.ToDateTime(classParam[4]);
                end = end.AddHours(Convert.ToInt32(classParam[7]));
                end = end.AddMinutes(Convert.ToInt32(classParam[8]));

                if (CheckHoursAvability(start, end))
                {
                    MainCollection.Main.Add(new TaskViewModel(classParam));
                }
            }

            SetDateRange("all");
        }
        /// <summary>
        /// Finish and delete task from list
        /// </summary>
        private void _FinishTask()
        {
            if(Index>=0)
                MainCollection.Main[Index].Status=TaskStatus.Finished;

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
            if (window.Name == "MainScreen") window.Visibility = Visibility.Hidden;
            else if (window != null) window.Close();
        }
        /// <summary>
        /// Save tasks to xml file while closing program
        /// </summary>
        private void Closing()
        {
            Serialization.SaveToXml(MainCollection.Main);
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
        /// Edit selected task
        /// </summary>
        /// <param name="param"></param>
        private void editTask(List<object> Prop)
        {
            MainCollection.Main[Index].Title = Prop[0].ToString();
            MainCollection.Main[Index].Location = Prop[1].ToString();
            MainCollection.Main[Index].IsRepeated = (bool)Prop[2];
            MainCollection.Main[Index].Interval = ((ComboBoxItem)Prop[3]).Content.ToString();

            DateTime start = Convert.ToDateTime(Prop[4]);
            start = start.AddHours(Convert.ToInt32(Prop[5]));
            start = start.AddMinutes(Convert.ToInt32(Prop[6]));
            DateTime end = Convert.ToDateTime(Prop[4]);
            end = end.AddHours(Convert.ToInt32(Prop[7]));
            end = end.AddMinutes(Convert.ToInt32(Prop[8]));

            if (CheckHoursAvability(start,end))
            {
                MainCollection.Main[Index].StartDate = start;

                MainCollection.Main[Index].EndDate = end;

                if (MainCollection.Main[Index].StartDate > MainCollection.Main[Index].EndDate) MainCollection.Main[Index].EndDate = MainCollection.Main[Index].StartDate.AddHours(1);
            }

            if(MainCollection.Main[Index].IsRepeated) MainCollection.Main[Index].SetNextNotifyDate();
            if (MainCollection.Main[Index].IsSynced) GoogleCalendarService.EditEvent(MainCollection.Main[Index]);

            SetDateRange("all");
        }
        private bool CheckHoursAvability(DateTime start,DateTime end)
        {
            foreach(TaskViewModel task in MainCollection.Main.Select(x=>x).Where(x=>x.StartDate.Day==start.Day))
            {
                if ((start.TimeOfDay < task.EndDate.TimeOfDay) && (task.StartDate.TimeOfDay > end.TimeOfDay))
                {
                    MessageBox.Show("Hours overlapping with " + task.Title + "(" + task.StartDate + " - " + task.EndDate, 
                        "Hours overlapping", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Check for tasks starting in max 1 hour
        /// </summary>
        private void CheckTasks(bool setTime)
        {
            foreach(TaskViewModel task in MainCollection.Main)
            {
                if (task.CheckDate()==TaskStatus.StartingSoon)
                {
                    MessageBox.Show("Your task: '" + task.Title + "' starting soon(" + task.StartDate.ToShortTimeString() + ")", "Your next task!",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if(task.CheckDate() == TaskStatus.InProgress)
                {
                    MessageBox.Show("Your task: '" + task.Title + "' is already in progress from ("+task.StartDate.ToShortTimeString()+")", "Your next task!",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (task.CheckDate() == TaskStatus.Finished)
                {
                    MessageBox.Show("Your task: '" + task.Title + "' finished at (" + task.EndDate.ToShortTimeString() + ")", "Your next task!",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    if (task.IsRepeated) task.SetNextNotifyDate();
                }

                if (task.EndDate <= DateTime.Now) task.Status = TaskStatus.Finished;
            }

            if(setTime) SetTimer(30);
        }
        /// <summary>
        /// Display tasks to do on this day
        /// </summary>
        private void CheckToday()
        {
            string msg = "Today tasks: ";
            foreach(TaskViewModel task in MainCollection.Main)
            {
                if (DateTime.Now.Day == task.StartDate.Day) msg += task.Title + " (" + task.StartDate.ToShortTimeString() + "), ";
            }

            MessageBox.Show(msg, "Today tasks", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /// <summary>
        /// Setting app to launch with system startup
        /// </summary>
        private void SetStartup()
        {
            
            try
            {
                if (!Settings.Default.IfStartup)
                {
                    WshShellClass wshShell = new WshShellClass();
                    IWshRuntimeLibrary.IWshShortcut shortcut;
                    string startUpFolderPath =
                      Environment.GetFolderPath(Environment.SpecialFolder.Startup);

                    // Create the shortcut
                    shortcut =
                      (IWshRuntimeLibrary.IWshShortcut)wshShell.CreateShortcut(
                        startUpFolderPath + "\\" +
                        Application.Current.MainWindow.GetType().Assembly + ".lnk");

                    shortcut.TargetPath = Assembly.GetExecutingAssembly().Location;
                    shortcut.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    shortcut.Description = "Launch My Application";
                    shortcut.IconLocation = AppDomain.CurrentDomain.BaseDirectory + @"\list.ico";
                    shortcut.Save();

                    MessageBox.Show("Application will be starting with system");
                }
                else
                {
                    string startUpFolderPath =
                    Environment.GetFolderPath(Environment.SpecialFolder.Startup);

                    DirectoryInfo di = new DirectoryInfo(startUpFolderPath);
                    FileInfo[] files = di.GetFiles("*.lnk");

                    foreach (FileInfo fi in files)
                    {
                        string shortcutTargetFile = GetShortcutTargetFile(fi.FullName);

                        if (shortcutTargetFile.EndsWith("TODOList.exe",
                              StringComparison.InvariantCultureIgnoreCase))
                        {
                            System.IO.File.Delete(fi.FullName);
                        }
                    }
                    MessageBox.Show("Application will not be longer starting with system");
                }

                Settings.Default.IfStartup = Settings.Default.IfStartup ? false : true;

                OnPropertyChange("StartupGraphics");

                Settings.Default.Save();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            

        }
        /// <summary>
        /// In order to determine if an existing shortcut 
        /// file is pointing to our application, we need to be able 
        /// to read the TargetPath from a shortcut file.
        /// </summary>
        /// <param name="shortcutFilename"></param>
        /// <returns></returns>
        public string GetShortcutTargetFile(string shortcutFilename)
        {
            string pathOnly = Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = Path.GetFileName(shortcutFilename);

            Shell32.Shell shell = new Shell32.ShellClass();
            Shell32.Folder folder = shell.NameSpace(pathOnly);
            Shell32.FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                Shell32.ShellLinkObject link =
                  (Shell32.ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }

            return String.Empty; // Not found
        }
        #endregion
        #region Timer
        /// <summary>
        /// Sets timer every half hour
        /// </summary>
        private void SetTimer(int minutes)
        {
            myTimer = new System.Timers.Timer(minutes * 60 * 1000); //one hour in milliseconds
            myTimer.Elapsed += new ElapsedEventHandler(TimerElapsing);
            myTimer.Start();
        }
        /// <summary>
        /// Method invoking when timer elapsed (Invoke method to check tasks)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerElapsing(object sender, ElapsedEventArgs e)
        {
            CheckTasks(true);
        }
        #endregion
    }
}
