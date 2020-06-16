using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOList
{
    public static class MainCollection
    {
        public static ObservableCollection<TaskViewModel> Main;

        public static void RemoveSelected(int Index)
        {
            if (Index >= 0)
                if (Main[Index].IsSynced) GoogleCalendarService.RemoveEvent(Main[Index]);
            Main.RemoveAt(Index);
        }
    }
}
