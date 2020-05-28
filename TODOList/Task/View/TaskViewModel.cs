﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

        public DateTime StartDate
        {
            get
            {
                return task.StartDate;
            }
            set
            {
                if (value != null)
                    task.StartDate = value;
                OnPropertyChange("AddDate");
            }
        }

        public DateTime EndDate
        {
            get
            {
                return task.EndDate;
            }
            set
            {
                if (value != null)
                    task.EndDate = value;
                OnPropertyChange("AddDate");
            }
        }

        public string DateString
        {
            get
            {
                return StartDate.ToString() + " - " + EndDate.ToString();
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

        public string Interval
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

        public string Organizer
        {
            get
            {
                return task.Organizer;
            }
            set
            {
                task.Organizer = value;

            }
        }

        public string GoogleID
        {
            get
            {
                return task.GoogleID;
            }
            set
            {
                task.GoogleID = value;
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
        /// <param name="Prop">List of properties</param>
        public TaskViewModel(List<object> Prop)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Title = Prop[0].ToString();
            this.Location = Prop[1].ToString();
            this.IsRepeated = (bool)Prop[2];
            this.Interval = ((ComboBoxItem)Prop[3]).Content.ToString();

            this.StartDate = Convert.ToDateTime(Prop[4]);
            this.StartDate = this.StartDate.AddHours(Convert.ToInt32(Prop[5]));
            this.StartDate = this.StartDate.AddMinutes(Convert.ToInt32(Prop[6]));

            this.EndDate = Convert.ToDateTime(Prop[4]);
            this.EndDate = this.EndDate.AddHours(Convert.ToInt32(Prop[7]));
            this.EndDate = this.EndDate.AddMinutes(Convert.ToInt32(Prop[8]));

            if (StartDate > EndDate) EndDate = StartDate.AddHours(1);

            this.Description = Prop[9].ToString();
            this.IsCompleted = DateTime.Now >= EndDate ? true : false;
            this.IsSynced = false;
            this.Attendees = new ObservableCollection<string>();
            SetNextNotifyDate();
        }

        #endregion
        #region Methods
        /// <summary>
        /// Set a date to next notify
        /// </summary>
        public void SetNextNotifyDate()
        {
            switch (Interval)
            {
                case "Every day":
                    this.NextNotifyDate.AddDays(1);
                    break;
                case "Every week":
                    this.NextNotifyDate.AddDays(7);
                    break;
                case "Every month":
                    this.NextNotifyDate.AddMonths(1);
                    break;
            }
            
        }

        public void CheckDate()
        {

        }
        #endregion
    }
}