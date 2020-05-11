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
        #region Fields and Properties

        public Guid id { get; set; }
        public string Title { get; set; }
        public DateTime AddDate { get; set; }

        private DateTime NextNotifyDate { get; set; }

        public bool IsRepeated { get; set; }



        public string AddDateString
        {
            get
            {
                return AddDate.ToShortDateString();
            }
        }
        public DateTime Deadline { get; set; }
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

        private bool _priority;
        public bool priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
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

        private ObservableCollection<SubTaskViewModel> subItems;
        public ObservableCollection<SubTaskViewModel> SubItems
        {
            get { return subItems ?? (subItems = new ObservableCollection<SubTaskViewModel>()); }
            set
            {
                subItems = value;
                OnPropertyChange("SubItems");
                OnPropertyChange("Count");
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
        public TaskViewModel(string Title,DateTime AddDate,string interval,bool priority)
        {
            this.id = Guid.NewGuid();
            this.Title = Title;
            this.AddDate = AddDate.Date;
            this.Deadline = Convert.ToDateTime(null);
            this.priority = priority;
            shouldVisible = false;
            this.IsRepeated = true;
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
            this.id = Guid.NewGuid();
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
            shouldVisible = subItems.Count > 0 ? true : false;
            OnPropertyChange("SubItemsVisible");
            OnPropertyChange("Completed");
            OnPropertyChange("CompletedString");
            OnPropertyChange("Count");
            OnPropertyChange("SubTaskString");
        }
        #endregion
    }
}
