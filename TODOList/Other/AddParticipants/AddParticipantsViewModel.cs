using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TODOList
{
    class AddParticipantsViewModel
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
            if (IsEmailValid(variables[0].ToString()))
            {
                if (!MainViewModel.Instance.Tasks[index].Attendees.Contains(variables[0].ToString()))
                    MainViewModel.Instance.Tasks[index].Attendees.Add(variables[0].ToString());
                else MessageBox.Show("This user is already participate in this event!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("E-mail address is not valide. Please check this and try again!", "Not valid mail", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ((TextBox)variables[1]).Text = String.Empty;
        }
        /// <summary>
        /// Checking if mail is valid
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <returns>True if mail is valid otherwise false</returns>
        private static bool IsEmailValid(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
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
