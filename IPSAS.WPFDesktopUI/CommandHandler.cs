using System;
using System.Windows.Input;

namespace IPSAS.WPFDesktopUI
{
    internal class CommandHandler : ICommand
    {
        private Func<object> action;
        private Func<bool> canExecute;

        public CommandHandler(Func<object> action, Func<bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public void Execute(object parameter)
        {
            action();
        }
    }
}
