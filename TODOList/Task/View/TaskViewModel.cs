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

        public string Id
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

        public string Location
        {
            get
            {
                return task.Location;
            }
            set
            {
                task.Location = value;
                OnPropertyChange("Location");
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

        public ObservableCollection<string> Attendees
        {
            get { return task.Attendees ?? (task.Attendees = new ObservableCollection<string>()); }
            set
            {
                task.Attendees = value;
                OnPropertyChange("Attendees");
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

        public bool IsCompleted
        {
            get
            {
                return task.IsCompleted;
            }
            set
            {
                task.IsCompleted = value;
                OnPropertyChange("IsCompleted");
            }
        }

        public string isCompleted
        {
            get
            {
                if (IsRepeated)
                {
                    return "Graphics/refresh.png";
                }
                else
                {
                    if (IsCompleted) return "Graphics/check.png";
                    else return "Graphics/square.png";
                }
            }
        }

        public string Description
        {
            get
            {
                return task.Description;
            }
            set
            {
                task.Description = value;
                OnPropertyChange("Description");
            }
        }

        public bool IsSynced
        {
            get
            {
                return task.IsSynced;
            }
            set
            {
                task.IsSynced = value;
                OnPropertyChange("IsSynced");
            }
        }

        public string isSynced
        {
            get
            {
                if (IsSynced) return "Graphics/google.png";
                else return "Graphics/google_black.png";
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
        /// <param name="IsRepeated"></param>
        public TaskViewModel(List<object> Prop)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Title = Prop[0].ToString();
            this.AddDate = DateTime.Now;
            this.Deadline = Convert.ToDateTime(null);
            this.IsRepeated = true;
            //this.Interval = interval;
            Attendees = new ObservableCollection<string>();
        }

        #endregion
        #region Methods
        #endregion
    }
}
