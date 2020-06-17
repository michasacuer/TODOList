using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace TODOList.Tests
{
    public class HoursCheckTests
    {
        [Theory]
        [InlineData("2020-06-18 10:40:00", "2020-06-18 12:50:00", false)]
        [InlineData("2020-06-18 18:00:00", "2020-06-18 20:00:00", false)]
        [InlineData("2020-06-18 08:30:00", "2020-06-18 10:50:00", true)]
        [InlineData("2020-06-19 08:30:00", "2020-06-19 10:50:00", true)]
        [InlineData("2020-06-18 09:30:00", "2020-06-18 12:50:00", false)]
        [InlineData("2020-06-18 17:30:00", "2020-06-18 19:50:00", false)]
        [InlineData("2020-06-18 23:30:00", "2020-06-19 00:50:00", false)]
        [InlineData("2020-06-18 01:30:00", "2020-06-19 02:30:00", false)]
        public void HoursAvailability_Test(string start,string end,bool expected)
        {

            ObservableCollection<TaskViewModel> collection = new ObservableCollection<TaskViewModel>
            {
                new TaskViewModel(new List<object> {"Task1","Location1",false,"None", "18-Jun-2020",1,30,2,30,"Desc1","New" }),
                new TaskViewModel(new List<object> {"Task1","Location1",false,"None", "18-Jun-2020", 2,40,3,30,"Desc1","New" }),
                new TaskViewModel(new List<object> {"Task1","Location1",false,"None", "18-Jun-2020", 12,40,13,30,"Desc1","New" }),
                new TaskViewModel(new List<object> {"Task1","Location1",false,"None", "18-Jun-2020", 18,40,23,30,"Desc1","New" }),
                new TaskViewModel(new List<object> {"Task1","Location1",false,"None", "18-Jun-2020", 15,30,16,30,"Desc1","New" }),
            };

            var Start = DateTime.ParseExact(start, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
            var End = DateTime.ParseExact(end, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
            HoursAvailability hours = new HoursAvailability();
            Assert.Equal(expected, hours.CheckHoursAvailability(collection, Start, End));
        }
    }
}
