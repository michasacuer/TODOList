using System;
using System.Windows.Input;

namespace TODOList
{
    public class NormalCommand : ICommand
    {
        private Action action;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        /// <summary>
        /// Create a new command
        /// </summary>
        /// <param name="action"></param>
        public NormalCommand(Action action)
        {
            this.action = action;
        }

        #region ICommand Members
        ///<summary>
        ///Defines the method that determines whether the command can execute in its current state.
        ///</summary>
        ///<param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        ///<returns>
        ///true
        ///</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        ///<summary>
        ///Defines the method to be called when the command is invoked.
        ///</summary>
        ///<param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        public void Execute(object parameter)
        {
            action();
        }
        #endregion
    }
}
