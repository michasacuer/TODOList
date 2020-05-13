using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOList
{
    class GoogleCalendarService
    {
        #region Variables
        GoogleAuth googleAuth;
        CalendarService service;
        string ApplicationName = "TODOList";
        #endregion
        #region Constructor
        public GoogleCalendarService()
        {
            //Create instance for googleAuth and new service
            googleAuth = new GoogleAuth();
            service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = googleAuth.GetCredential(),
                ApplicationName = this.ApplicationName
            });
        }
        #endregion
        #region Methods
        public void InsertIntoCalendar(TaskViewModel task)
        {
            Event addEvent = new Event();
            addEvent.Id = task.Id;
            addEvent.Summary = task.Title;
            addEvent.Location = task.Location;
            addEvent.Description = task.Description;
            addEvent.Start = new EventDateTime()
            {
                DateTime = task.Deadline.AddHours(-1)
            };
            addEvent.End = new EventDateTime()
            {
                DateTime = task.Deadline
            };
            
        }
        #endregion
    }
}
