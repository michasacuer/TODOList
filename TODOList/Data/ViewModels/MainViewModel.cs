using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace TODOList
{
    internal class MainViewModel:BaseViewModel
    {
        #region Commands
        public ICommand SelectListItem { get; set; }
        public ICommand SelectSubListItem { get; set; }
        public ICommand Open { get; set; }
        public ICommand MarkAsCompleted { get; set; }
        public ICommand Remove { get; set; }
        public ICommand FinishTask { get; set; }
        public ICommand CloseApp { get; set; }
        public ICommand Minimize { get; set; }
        public ICommand Confirm { get; set; }

        public ICommand WindowClosing { get; set; }
        public ICommand PriorityChange { get; set; }
        #endregion

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

        private ItemViewModel SelectedItem;
        public ItemViewModel Selected
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
        private ObservableCollection<ItemViewModel> tasks;
        public ObservableCollection<ItemViewModel> Tasks
        {
            get { return tasks ?? (tasks = new ObservableCollection<ItemViewModel>()); }
            set { tasks = value; OnPropertyChange("Tasks"); }
        }

        public static MainViewModel Instance = new MainViewModel();

        public MainViewModel()
        {
            Selected = null;
            Confirm = new RelayCommand<Object[]>(NewTask);
            Open = new RelayCommand<string>(OpenWindow);
            MarkAsCompleted = new RelayCommand<Int32>(_MarkAsCompleted);
            Remove = new RelayCommand<Int32>(_Remove);
            FinishTask = new NormalCommand(_FinishTask);
            CloseApp = new RelayCommand<Window>(Close);
            Minimize = new RelayCommand<Window>(minimize);
            WindowClosing = new NormalCommand(Closing);
            PriorityChange = new NormalCommand(ChangePriority);

            if (File.Exists(XmlFilePath))
            {
                LoadFromXml();
            }
            else
            {
                Tasks = new ObservableCollection<ItemViewModel>();
                Tasks.Add(new ItemViewModel("sd", DateTime.Now, DateTime.Now, true));
            }

            //RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            //registryKey.SetValue("ApplicationName", System.Reflection.Assembly.GetExecutingAssembly().Location);

        }

        private void OpenWindow(string tag)
        {
            NewTask AddNew = new NewTask();
            if (tag == "SubItem") AddNew.Extended.Visibility = Visibility.Hidden;

            AddNew.Confirm.Tag = tag;
            AddNew.Show();
        }

        public void NewTask(object[] param)
        {
            if (param[1].ToString() == "SubItem")
            {
                
                Tasks[Index].SubItems.Add(new SubItemViewModel(param[2].ToString()));
                Tasks[Index].CheckCompletion();
            }
            else
            {
                Tasks.Add(new ItemViewModel(param[2].ToString(), DateTime.Now, Convert.ToDateTime(param[3]), (bool)param[4]));
            }
            (param[0] as Window).Close();
        }

        private void _MarkAsCompleted(int ind)
        {
            if (ind > -1)
            {
                Tasks[Index].SubItems[ind].IsCompleted = true;
                Tasks[Index].CheckCompletion();
            }
            
        }

        private void _Remove(int index)
        {
            if (index > -1)
            {
                Tasks[Index].SubItems.RemoveAt(index);
                Tasks[Index].CheckCompletion();
            }
            
        }

        private void _FinishTask()
        {
            Tasks.RemoveAt(Index);
        }

        private void minimize(Window window)
        {
            if (window != null) window.WindowState = WindowState.Minimized;
        }

        private void Close(Window window)
        {
            if (window != null) window.Close();
        }

        private void ChangePriority()
        {
            Tasks[Index].priority = Tasks[Index].priority==true?false:true;
        }

        private void Closing()
        {
            SaveToXml();
        }

        private void SaveToXml()
        {
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(ObservableCollection<ItemViewModel>));

            var path = XmlFilePath;
            FileStream file = System.IO.File.Create(path);
            writer.Serialize(file, Tasks);
            file.Close();
        }

        private void LoadFromXml()
        {
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(ObservableCollection<ItemViewModel>));
            System.IO.StreamReader file = new StreamReader(XmlFilePath);
            Tasks = reader.Deserialize(file) as ObservableCollection<ItemViewModel>;
            file.Close();
        }
    }
}
