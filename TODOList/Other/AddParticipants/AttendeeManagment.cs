using System.Collections.ObjectModel;

namespace TODOList
{
    public static class AttendeeManagment
    {
        public static void Add(ObservableCollection<string> AttendeeList, string mail)
        {
            AttendeeList.Add(mail);
        }

        public static void Remove(ObservableCollection<string> AttendeeList, int index)
        {
            AttendeeList.RemoveAt(index);
        }
    }
}
