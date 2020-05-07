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
    public class ItemViewModel:BaseViewModel
    {
        #region Fields and Properties
        public string Title { get; set; }
        public DateTime AddDate { get; set; }
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
                return Deadline.ToShortDateString();
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

        private ObservableCollection<SubItemViewModel> subItems;
        public ObservableCollection<SubItemViewModel> SubItems
        {
            get { return subItems ?? (subItems = new ObservableCollection<SubItemViewModel>()); }
            set
            {
                subItems = value;
                OnPropertyChange("SubItems");
                OnPropertyChange("Count");
            }
        }
        #endregion

        #region Constructor
        public ItemViewModel()
        {

        }
        public ItemViewModel(string Title,DateTime AddDate,DateTime Deadline,bool priority)
        {
            this.Title = Title;
            this.AddDate = AddDate.Date;
            this.Deadline = Deadline;
            this.priority = priority;
            shouldVisible = false;
            SubItems = new ObservableCollection<SubItemViewModel>();
            CheckCompletion();
        }
        #endregion
        #region Methods
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
