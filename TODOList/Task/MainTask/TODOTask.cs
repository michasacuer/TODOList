using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOList
{
    class TODOTask
    {
        public Guid id;
        public string Title;
        public DateTime AddDate;
        public DateTime NextNotifyDate;
        public bool IsRepeated;
        public DateTime Deadline;
        public int interval;
        public ObservableCollection<SubTaskViewModel> subItems;
        public bool _priority;
    }
}
