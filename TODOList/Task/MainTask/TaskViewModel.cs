using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TODOList
{
    [Serializable()]
    public class TaskViewModel:BaseViewModel
    {
        TODOTask task = new TODOTask();

        #region Fields and Properties

        public Guid Id
        {
            get
            {
                return task.id;
            }
            set
            {
                if (value != null)
                    task.id = value;
                OnPropertyChange("GetId");
            }
        }

        public string Title
        {
            get
            {
                return task.Title;
            }
            set
            {
                if (value != null)
                    task.Title = value;
                OnPropertyChange("Title");
            }
        }

        public DateTime AddDate
        {
            get
            {
                return task.AddDate;
            }
            set
            {
                if (value != null)
                    task.AddDate = value;
                OnPropertyChange("AddDate");
            }
        }

        public string AddDateString
        {
            get
            {
                return AddDate.ToShortDateString();
            }
        }

        public DateTime Deadline
        {
            get
            {
                return task.Deadline;
            }
            set
            {
                task.Deadline = value;
                OnPropertyChange("Deadline");
            }
        }
        public string DeadlineString
        {
            get
            {
                if (Deadline != null) return Deadline.ToShortDateString();
                else return "-";
            }
        }
        public int Count
        {
            get
            {
                return SubItems.Count;
            }
        }
        private int completed;
        public int Completed
        {
            get
            {
                return completed;
            }
            set
            {
                completed = value;
                OnPropertyChange("Completed");
                OnPropertyChange("CompletedString");
            }
        }

        public string CompletedString
        {
            get
            {
                return Completed + "/" + Count;
            }
        }
        public string SubTaskString
        {
            get
            {
                return "Completed subtask: "+Completed+"/"+Count;
            }
        }

        public bool IsRepeated
        {
            get
            {
                return task.IsRepeated;
            }
            set
            {
                task.IsRepeated = value;
                OnPropertyChange("IsRepeated");
            }
        }

        public bool priority
        {
            get
            {
                return task._priority;
            }
            set
            {
                task._priority = value;
                OnPropertyChange("priority");
            }
        }

        private bool shouldVisible;
        public Visibility SubItemsVisible
        {
            get
            {
                return shouldVisible ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public string Priority
        {
            get
            {
                if(priority) return "Graphics/starC.png";
                else return "Graphics/starW.png";
            }
        }

        public ObservableCollection<SubTaskViewModel> SubItems
        {
            get { return task.subItems ?? (task.subItems = new ObservableCollection<SubTaskViewModel>()); }
            set
            {
                task.subItems = value;
                OnPropertyChange("SubItems");
                OnPropertyChange("Count");
            }
        }

        public DateTime NextNotifyDate
        {
            get
            {
                return task.NextNotifyDate;
            }
            set
            {
                task.NextNotifyDate = value;
                OnPropertyChange("NextNotifyDate");
            }
        }

        public int Interval
        {
            get
            {
                return task.interval;
            }
            set
            {
                task.interval = value;
                OnPropertyChange("Interval");
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor without parameter
        /// </summary>
        public TaskViewModel()
        {

        }

        /// <summary>
        /// Constructor that is used for repetitive tasks
        /// </summary>
        /// <param name="Title">Task title</param>
        /// <param name="AddDate">Date when task was added</param>
        /// <param name="interval">String to set repetitive interval</param>
        /// <param name="priority">Boolean to set positioning of task</param>
        public TaskViewModel(string Title,DateTime AddDate,int interval,bool priority)
        {
            this.Id = Guid.NewGuid();
            this.Title = Title;
            this.AddDate = AddDate.Date;
            this.Deadline = Convert.ToDateTime(null);
            this.priority = priority;
            this.shouldVisible = false;
            this.IsRepeated = true;
            this.Interval = interval;
            SubItems = new ObservableCollection<SubTaskViewModel>();
            CheckCompletion();
        }

        /// <summary>
        /// Constructor that is used for non-repetitive tasks
        /// </summary>
        /// <param name="Title">Task title</param>
        /// <param name="AddDate">Date when task was added</param>
        /// <param name="Deadline">Date by which the task should be completed</param>
        /// <param name="priority">Boolean to set positioning of task</param>
        public TaskViewModel(string Title,DateTime AddDate,DateTime Deadline,bool priority)
        {
            this.Id = Guid.NewGuid();
            this.Title = Title;
            this.AddDate = AddDate.Date;
            this.Deadline = Deadline;
            this.priority = priority;
            this.IsRepeated = false;
            shouldVisible = false;
            SubItems = new ObservableCollection<SubTaskViewModel>();
            CheckCompletion();
        }
        #endregion
        #region Methods
        /// <summary>
        /// Method that is checking completed task
        /// and infrom view about that
        /// </summary>
        public void CheckCompletion()
        {
            Completed = SubItems.Where(x => x.IsCompleted == true).Count();
            this.shouldVisible = SubItems.Count > 0 ? true : false;
            OnPropertyChange("SubItemsVisible");
            OnPropertyChange("Completed");
            OnPropertyChange("CompletedString");
            OnPropertyChange("Count");
            OnPropertyChange("SubTaskString");
        }
        #endregion
    }
}
