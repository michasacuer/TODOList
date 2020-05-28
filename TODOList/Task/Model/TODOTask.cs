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
        public string id;
        public string Title;
        public DateTime StartDate;
        public DateTime EndDate;
        public DateTime NextNotifyDate;
        public bool IsRepeated;
        public string interval;
        public ObservableCollection<string> Attendees;
        public string Location;
        public bool IsCompleted;
        public string Description;
        public bool IsSynced;
        public string Organizer;
        public string GoogleID;
    }
}
