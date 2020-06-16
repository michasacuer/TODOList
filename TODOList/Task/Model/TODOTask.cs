using System;
using System.Collections.ObjectModel;

namespace TODOList
{
    class TODOTask
    {
        public string id;
        public string Title;
        public DateTime StartDate;
        public DateTime EndDate;
        public bool IsRepeated;
        public string interval;
        public ObservableCollection<string> Attendees;
        public string Location;
        public TaskStatus Status;
        public string Description;
        public bool IsSynced;
        public string Organizer;
        public string GoogleID;
    }
}
