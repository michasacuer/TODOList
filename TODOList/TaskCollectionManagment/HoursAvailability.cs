using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TODOList
{
    public class HoursAvailability
    {
        /// <summary>
        /// Check if task overlapping
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public bool CheckHoursAvailability(ObservableCollection<TaskViewModel> collection,DateTime start, DateTime end)
        {
            List<TaskViewModel> ListToCheck = collection.Where(x => x.StartDate.Day == start.Day).ToList();
            foreach (TaskViewModel task in ListToCheck)
            {
                if (start <= task.EndDate && end >= task.StartDate)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
