using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOList
{
    class TODOItem
    {
        public string Title { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime Deadline { get; set; }
        public int Count { get; set; }
        public int Percent { get; set; }
        public ObservableCollection<TODOSubItem> subItems { get; set; }
    }
}
