using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TODOList
{
    public class AddParticipantsViewModel
    {

        #region Variable
        private int index;
        #endregion
        #region Commands
        public ICommand CloseWindow { get; set; }
        public ICommand AddAttendees { get; set; }
        #endregion
        #region Constructor
        public AddParticipantsViewModel(int index)
        {
            this.CloseWindow = new RelayCommand<Window>(Close);
            this.AddAttendees = new RelayCommand<object[]>(Add);
            this.index = index;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Method to add participants to event
        /// </summary>
        /// <param name="variables"></param>
        private void Add(object[] variables)
        {
            if (MailValidation.IsEmailValid(variables[0].ToString()))
            {
                if (!MainCollection.Main[index].Attendees.Contains(variables[0].ToString()))
                    AttendeeManagment.Add(MainCollection.Main[index].Attendees, variables[0].ToString());
                else MessageBox.Show("This user is already participate in this event!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("E-mail address is not valid. Please check this and try again!", "Not valid mail", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ((TextBox)variables[1]).Text = String.Empty;
        }
        
        /// <summary>
        /// Close window
        /// </summary>
        /// <param name="window">Determines which window close</param>
        private void Close(Window window)
        {
            if (window != null) window.Close();
        }
        #endregion
    }
}
