using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        /// <summary>
        /// Insert task into Google Calendar
        /// </summary>
        /// <param name="task"></param>
        public void InsertIntoCalendar(TaskViewModel task)
        {
            try
            {
                Event addEvent = new Event();
                addEvent.Summary = task.Title;
                addEvent.Location = task.Location;
                addEvent.Description = task.Description;
                addEvent.Start = new EventDateTime()
                {
                    DateTime = task.StartDate,
                    TimeZone = "Europe/Warsaw"

                };
                addEvent.End = new EventDateTime()
                {
                    DateTime = task.EndDate,
                    TimeZone = "Europe/Warsaw"
                };
                addEvent.Attendees = task.Attendees.Select(x => new EventAttendee() { Email = x.ToString() }).ToList();
                addEvent.Reminders = new Event.RemindersData()
                {
                    UseDefault = false,
                    Overrides = new EventReminder[]
                    {
                    new EventReminder() { Method="email",Minutes=24*60},
                    new EventReminder(){ Method="sms",Minutes=60}
                    }
                };
                if (task.IsRepeated)
                {
                    switch (task.Interval)
                    {
                        case "Every day":
                            addEvent.Recurrence = new string[] { "RRULE:FREQ=DAILY;INTERVAL=1;COUNT=2" };
                            break;
                        case "Every week":
                            addEvent.Recurrence = new string[] { "RRULE:FREQ=WEEKLY;INTERVAL=1" };
                            break;
                        case "Every month":
                            addEvent.Recurrence = new string[] { "RRULE:FREQ=MONTHLY;INTERVAL=1" };
                            break;
                    }
                }
                EventsResource.InsertRequest insert = service.Events.Insert(addEvent, "primary");
                insert.SendNotifications = true;
                insert.Execute();
                task.GoogleID = getGoogleId(task.Title);
            }
            catch (Google.GoogleApiException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Check if event exists in Google Calendar
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool CheckIfEventExists(TaskViewModel task)
        {
            EventsResource.ListRequest request = service.Events.List("primary");
            Events events = request.Execute();
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach(var eventItem in events.Items)
                {
                    if (eventItem.Summary == task.Title) return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Remove event from Google Calendar
        /// </summary>
        /// <param name="task"></param>
        public void RemoveEvent(TaskViewModel task)
        {
            if (task.IsRepeated)
            {
                Events instances= service.Events.Instances("primary", task.GoogleID).Execute(); ;

                if(instances!=null)
                    foreach(Event instance in instances.Items)
                    {
                        instance.Status = "cancelled";
                        service.Events.Update(instance, "primary", instance.Id).Execute();
                    }
            }
            else
            {
                service.Events.Delete("primary", task.GoogleID).Execute();
            }
            
        }
        /// <summary>
        /// Edit task synced with Google Calendar
        /// </summary>
        /// <param name="task"></param>
        public void EditEvent(TaskViewModel task)
        {
            try
            {
                Event editEvent = service.Events.Get("primary", task.GoogleID).Execute();
                editEvent.Summary = task.Title;
                editEvent.Location = task.Location;
                editEvent.Description = task.Description;
                editEvent.Start = new EventDateTime()
                {
                    DateTime = task.StartDate,
                    TimeZone = "Europe/Warsaw"

                };
                editEvent.End = new EventDateTime()
                {
                    DateTime = task.EndDate,
                    TimeZone = "Europe/Warsaw"
                };
                editEvent.Attendees = task.Attendees.Select(x => new EventAttendee() { Email = x.ToString() }).ToList();
                editEvent.Reminders = new Event.RemindersData()
                {
                    UseDefault = false,
                    Overrides = new EventReminder[]
                    {
                    new EventReminder() { Method="email",Minutes=24*60},
                    new EventReminder(){ Method="sms",Minutes=60}
                    }
                };
                if (task.IsRepeated)
                {
                    switch (task.Interval)
                    {
                        case "Every day":
                            editEvent.Recurrence = new string[] { "RRULE:FREQ=DAILY;INTERVAL=1;COUNT=2" };
                            break;
                        case "Every week":
                            editEvent.Recurrence = new string[] { "RRULE:FREQ=WEEKLY;INTERVAL=1" };
                            break;
                        case "Every month":
                            editEvent.Recurrence = new string[] { "RRULE:FREQ=MONTHLY;INTERVAL=1" };
                            break;
                    }
                }
                service.Events.Update(editEvent, "primary", task.GoogleID).Execute();
            }
            catch (Google.GoogleApiException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Get event id by title
        /// </summary>
        /// <param name="title"></param>
        private string getGoogleId(string title)
        {
            EventsResource.ListRequest request = service.Events.List("primary");
            Events events = request.Execute();
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    if (eventItem.Summary == title) return eventItem.Id;
                }
            }
            return String.Empty;
        }
        #endregion
    }
}
