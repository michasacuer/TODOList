using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TODOList
{
    class InfoViewModel:BaseViewModel
    {
        #region Commands
        public ICommand close { get; set; }
        #endregion

        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        #region Constructor
        public InfoViewModel()
        {
            this.close = new RelayCommand<Window>(CloseWindow);
            Text = "Created by: Mateusz Trybuła /nIcons and graphics: https://icons8.com/icons /n@jakub.dmuchowski";
        }
        #endregion

        #region Methods
        /// <summary>
        /// This method close window
        /// </summary>
        /// <param name="window">Determines window to close</param>
        private void CloseWindow(Window window)
        {
            if (window != null) window.Close();
        }
        #endregion

    }
}
