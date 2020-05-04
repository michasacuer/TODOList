using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOList
{
    public class SubItemViewModel:BaseViewModel
    {
        public string Title { get; set; }
        private bool isCompleted;
        public bool IsCompleted
        {
            get
            {
                return isCompleted;
            }
            set
            {
                isCompleted = value;
                OnPropertyChange();
            }
        }

        public SubItemViewModel() { }
        public SubItemViewModel(string Title)
        {
            this.Title = Title;
            this.IsCompleted = false;
        }
    }
}
