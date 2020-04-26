using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOList
{
    class ItemViewModel:BaseViewModel
    {
        public string Title { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime Deadline { get; set; }
        public int Count { get; set; }
        public int Percent { get; set; }
        public ObservableCollection<TODOSubItem> SubItems { get; set; }

        public ItemViewModel(string Title,DateTime AddDate,DateTime Deadline)
        {
            this.Title = Title;
            this.AddDate = AddDate.Date;
            this.Deadline = Deadline;
            Count = 1;
            Percent = 0;
        }
    }
}
