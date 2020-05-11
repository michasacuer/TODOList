using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOList
{
    public class SubTaskViewModel:BaseViewModel
    {
        public Guid id { get; set; }
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
                OnPropertyChange("IsCompleted");
                OnPropertyChange("image");
            }
        }

        public string image
        {
            get
            {
                if (IsCompleted) return "Graphics/check.png";
                else return "Graphics/square.png";
            }
        }

        public SubTaskViewModel() { }
        public SubTaskViewModel(string Title)
        {
            this.id = Guid.NewGuid();
            this.Title = Title;
            this.IsCompleted = false;
        }
    }
}
