using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UserInterface.Command
{
    public class RelayCommandAsync : ICommand
    {
        private readonly Func<Task> executedMethod;
        private readonly Func<bool> canExecuteMethod;

        public event EventHandler CanExecuteChanged;
        public RelayCommandAsync(Func<Task> execute) : this(execute, null) { }

        public RelayCommandAsync(Func<Task> execute, Func<bool> canExecute)
        {
            this.executedMethod = execute ?? throw new ArgumentNullException("execute");
            this.canExecuteMethod = canExecute;
        }

        public bool CanExecute(object parameter) => this.canExecuteMethod == null || this.canExecuteMethod();
        public async void Execute(object parameter) => await this.executedMethod();
        public void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

}