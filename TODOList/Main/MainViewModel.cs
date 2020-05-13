using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;

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
        #endregion

        #region Fields and Properties
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
            Confirm = new RelayCommand<List<object>>(NewTask);
            Open = new RelayCommand<string>(OpenWindow);
            FinishTask = new NormalCommand(_FinishTask);
            CloseApp = new RelayCommand<Window>(Close);
            Minimize = new RelayCommand<Window>(minimize);
            WindowClosing = new NormalCommand(Closing);
            OpenInfo = new NormalCommand(openInfo);

            if (File.Exists(XmlFilePath))
            {
                LoadFromXml();
            }
            else
            {
                Tasks = new ObservableCollection<TaskViewModel>();
            }

            //RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            //registryKey.SetValue("ApplicationName", System.Reflection.Assembly.GetExecutingAssembly().Location);

        }
        #endregion

        #region Methods
        /// <summary>
        /// Open NewTask window to add task or subtask
        /// </summary>
        /// <param name="tag">Determines which one, task or subtask, want to add</param>
        private void OpenWindow(string tag)
        {
            NewTask AddNew = new NewTask();
            if (tag == "SubItem") AddNew.Extended.Visibility = Visibility.Hidden;

            AddNew.Confirm.Tag = tag;
            AddNew.Show();
        }
        /// <summary>
        /// Add new task or subtask
        /// </summary>
        /// <param name="param">Object array with parameters to add new items</param>
        public void NewTask(List<object> param)
        {
            (param[0] as Window).Close();
            param.RemoveAt(0);
            Tasks.Add(new TaskViewModel(param));
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
        /// Close window
        /// </summary>
        /// <param name="window">Determines which window close</param>
        private void Close(Window window)
        {
            if (window != null) window.Close();
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
