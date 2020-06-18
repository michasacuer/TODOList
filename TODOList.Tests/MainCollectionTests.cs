using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace TODOList.Tests
{
    public class MainCollectionTests
    {
        [Fact]
        public void Add_Test()
        {
            MainCollection.service = new GoogleCalendarService(null);
            MainCollection.Main = new ObservableCollection<TaskViewModel>();

            List<object> NewTaskParameters = new List<object>
            {
                "Task1","Location1",false,"None",DateTime.Now.ToShortDateString(),1,30,2,30,"Desc1","New"
            };
            Assert.True(MainCollection.Add(NewTaskParameters, true));
            Assert.False(MainCollection.Add(NewTaskParameters, false));

            List<object> NewTaskParameters2 = new List<object>
            {
                "Task2","Location1",false,"None",DateTime.Now.ToShortDateString(),1,30,2,30,"Desc1","New"
            };

            MainCollection.Main.Add(new TaskViewModel(NewTaskParameters2));

            Assert.Equal(2, MainCollection.Main.Count);
            Assert.DoesNotContain(new TaskViewModel(NewTaskParameters2), MainCollection.Main);
        }

        [Fact]
        public void Remove_Test()
        {
            MainCollection.service = new GoogleCalendarService(null);
            MainCollection.Main = new ObservableCollection<TaskViewModel>
            {
                new TaskViewModel(new List<object> {"Task1","Location1",false,"None",DateTime.Now.ToShortDateString(),1,30,2,30,"Desc1","New" }),
                new TaskViewModel(new List<object> {"Task1","Location1",false,"None",DateTime.Now.ToShortDateString(),1,30,2,30,"Desc1","New" }),
            };

            Assert.NotEmpty(MainCollection.Main);

            var tempTask = MainCollection.Main[0];
            MainCollection.RemoveSelected(0);

            Assert.Single(MainCollection.Main);

            Assert.NotEqual(tempTask, MainCollection.Main[0]);

            MainCollection.RemoveSelected(0);

            Assert.Empty(MainCollection.Main);
        }

        [Fact]
        public void Edit_Test()
        {
            MainCollection.service = new GoogleCalendarService(null);
            MainCollection.Main = new ObservableCollection<TaskViewModel>
            {
                new TaskViewModel(new List<object> {"Task1","Location1",false,"None",DateTime.Now.ToShortDateString(),1,30,2,30,"Desc1","New" }),
                new TaskViewModel(new List<object> {"Task1","Location1",false,"None",DateTime.Now.ToShortDateString(),1,30,2,30,"Desc1","New" }),
            };

            Assert.NotEmpty(MainCollection.Main);

            List<object> EditParameters = new List<object>
            {
                "TaskEdited","Location1",false,"None",DateTime.Now.ToShortDateString(),1,30,2,30,"Desc1","New",0
            };

            List<object> EditParameters2 = new List<object>
            {
                "Task2Edited","Location2",true,"Every day",DateTime.Now.ToShortDateString(),2,30,3,30,"Desc1","New",1
            };

            MainCollection.editTask(EditParameters, true);
            MainCollection.editTask(EditParameters2, true);

            Assert.Equal("TaskEdited", MainCollection.Main[0].Title);
            Assert.Equal("18-Jun-20 02:30:00 - 18-Jun-20 03:30:00", MainCollection.Main[1].DateString);
        }
    }
}
