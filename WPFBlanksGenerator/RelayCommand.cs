using System;
using System.Windows.Input;

namespace WPFBlanksGenerator
{
    public class RelayCommand<T> : ICommand
    {
        protected Action<T> execute;
        protected Func<T, bool> canExecute;

        public RelayCommand(Action<T> execute) : this(execute, (Func<T, bool>)null) { }

        public RelayCommand(Action<T> execute, Func<bool> canExecute) : this(execute, t => canExecute()) { }

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute ?? (t => true);
        }

        public RelayCommand() { }

        public void Execute(object parameter) => execute((T)parameter);
        public bool CanExecute(object parameter) => canExecute((T)parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action execute) : base(t => execute()) { }
        public RelayCommand(Action<object> execute) : base(execute) { }
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute) : base(execute, canExecute) { }
        public RelayCommand(Action execute, Func<bool> canExecute) : base(t => execute(), t => canExecute()) { }
        public RelayCommand(Action<object> execute, Func<bool> canExecute) : base(execute, canExecute) { }
    }
}