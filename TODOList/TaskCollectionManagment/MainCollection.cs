using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TODOList
{
    public static class MainCollection
    {
        public static ObservableCollection<TaskViewModel> Main;

        public static GoogleCalendarService service = new GoogleCalendarService();

        public static bool RemoveSelected(int Index)
        {

            bool IfUnsynced = true;
            if (Index >= 0)
                if (Main[Index].IsSynced) IfUnsynced = service.RemoveEvent(Main[Index]);
            if (IfUnsynced)
            {
                Main.RemoveAt(Index);
                return true;
            }

            return false;
        }

        public static bool Add(List<object> param,bool HourAvailable)
        {
            if (param[4] == null) { param[4] = DateTime.Now.ToShortDateString(); param[5] = DateTime.Now.Hour + 1; }
            if (param[0].ToString() == String.Empty) param[0] = new StringBuilder("(Empty subject)");
            if (param[10].ToString() == "Edit")
            {
                editTask(param,HourAvailable);
                return true;
            }
            else
            {
                
                if (HourAvailable)
                {
                    Main.Add(new TaskViewModel(param));
                    return true;
                }
                return false;
            }
        }

        public static void editTask(List<object> Prop, bool HourAvailable)
        {

            int Index = Convert.ToInt32(Prop[11]);
            Main[Index].Title = Prop[0].ToString();
            Main[Index].Location = Prop[1].ToString();
            Main[Index].IsRepeated = (bool)Prop[2];
            Main[Index].Interval = Prop[3].ToString();

            DateTime start = Convert.ToDateTime(Prop[4]);
            start = start.AddHours(Convert.ToInt32(Prop[5]));
            start = start.AddMinutes(Convert.ToInt32(Prop[6]));
            DateTime end = Convert.ToDateTime(Prop[4]);
            end = end.AddHours(Convert.ToInt32(Prop[7]));
            end = end.AddMinutes(Convert.ToInt32(Prop[8]));

            if (HourAvailable)
            {
                Main[Index].StartDate = start;

                Main[Index].EndDate = end;

                if (Main[Index].StartDate > Main[Index].EndDate) Main[Index].EndDate = Main[Index].StartDate.AddHours(1);
            }

            if (Main[Index].IsRepeated) Main[Index].SetNextNotifyDate();
            if (Main[Index].IsSynced) service.EditEvent(Main[Index]);
        }
    }
}
