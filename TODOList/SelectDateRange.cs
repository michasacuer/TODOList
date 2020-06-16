using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOList
{
    public static class SelectDateRange
    {
        public static ObservableCollection<TaskViewModel> SelectTaskFromRange(ObservableCollection<TaskViewModel> tasks, string range)
        {
            tasks = new ObservableCollection<TaskViewModel>(tasks.OrderBy(i => i.StartDate).ToList());
            switch (range)
            {
                default:
                    return tasks;
                case "this day":
                    return new ObservableCollection<TaskViewModel>(tasks.Where(i => i.StartDate.Day == DateTime.Now.Day).ToList());
                case "next day":
                    return new ObservableCollection<TaskViewModel>(tasks.Where(i => i.StartDate.Day == DateTime.Now.AddDays(1).Day).ToList());
                case "this week":
                    return new ObservableCollection<TaskViewModel>(tasks.Where(i => (i.StartDate >= DateTime.Now
                    && i.StartDate <= DateTime.Now.AddDays(7))).ToList());
                case "next week":
                    return new ObservableCollection<TaskViewModel>(tasks.Where(i => (i.StartDate >= DateTime.Now.AddDays(7)
                    && i.StartDate <= DateTime.Now.AddDays(14))).ToList());
                case "this month":
                    return new ObservableCollection<TaskViewModel>(tasks.Where(i => i.StartDate.Month == DateTime.Now.Month).ToList());
                case "next month":
                    return new ObservableCollection<TaskViewModel>(tasks.Where(i => i.StartDate.Month == DateTime.Now.AddMonths(1).Month).ToList());
            }
        }
    }
}
