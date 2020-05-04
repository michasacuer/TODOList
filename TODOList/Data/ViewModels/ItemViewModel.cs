using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return SubItems.Count + 1;
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
                OnPropertyChange();
            }
        }

        public string CompletedString
        {
            get
            {
                return Completed + "/" + Count;
            }
        }
        public int Percent
        {
            get
            {
                return Completed/Count*100;
            }
        }

        public string PercentString
        {
            get
            {
                return Percent.ToString()+"%";
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
                OnPropertyChange();
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
        public ObservableCollection<SubItemViewModel> SubItems { get; set; }
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
            SubItems = new ObservableCollection<SubItemViewModel>();
        }
        #endregion
        #region Methods
        public void CheckCompletion()
        {
            Completed = SubItems.Where(x => x.IsCompleted == true).Count();
        }
        #endregion
    }
}
