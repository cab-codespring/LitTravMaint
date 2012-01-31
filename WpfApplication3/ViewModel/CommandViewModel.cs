using System;
using System.Windows.Input;
using ReactiveUI;

namespace LitTravProj.ViewModel
{
    /// <summary>
    /// Represents an actionable item displayed by a View.
    /// </summary>
    public class CommandViewModel : ReactiveObject

    {
        public CommandViewModel(string displayName, ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

          //  base.DisplayName = displayName;
            this.Command = command;
        }

        public ICommand Command { get; private set; }
    }
}