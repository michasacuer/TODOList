using System;
using System.Collections.ObjectModel;

namespace TODOList
{
    class TODOTask
    {
        // gettery, settery, z wielkiej litery jak publiczne
        public string id;
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsRepeated { get; set; }
        public string interval;
        public ObservableCollection<string> Attendees { get; set; }
        public string Location { get; set; }
        public TaskStatus Status { get; set; }
        public string Description { get; set; }
        public bool IsSynced { get; set; }
        public string Organizer { get; set; }
        public string GoogleID { get; set; }
    }
}
