using Xunit;

namespace TODOList.Tests
{
    public class AddParticipantsViewModelTests
    {
        [Theory]
        [InlineData("testdata", false)]
        [InlineData("test.data", false)]
        [InlineData("test%data", false)]
        [InlineData("test@data.pl", true)]
        public void IsMailValid_Test(string mail, bool expected)
        {
            Assert.Equal(expected, MailValidation.IsEmailValid(mail));
        }

        [Fact]
        public void AttendeeManagement_Test()
        {
            var task = new TaskViewModel();

            AttendeeManagment.Add(task.Attendees, "mail@mail.com");
            AttendeeManagment.Add(task.Attendees, "test@mail.com");
            AttendeeManagment.Add(task.Attendees, "test2@mail.com");

            Assert.Equal(3, task.Attendees.Count);
            Assert.Contains("test@mail.com", task.Attendees);
            AttendeeManagment.Remove(task.Attendees, 1);
            Assert.DoesNotContain("test@mail.com", task.Attendees);
            Assert.Equal(2, task.Attendees.Count);
        }
    }
}
