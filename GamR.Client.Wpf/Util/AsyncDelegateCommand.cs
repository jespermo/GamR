using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GamR.Client.Wpf.Util
{
    public class AsyncDelegateCommand<TArgetType> : IAsyncCommand
    {
        protected readonly Predicate<TArgetType> _canExecute;
        protected Func<TArgetType, Task> _asyncExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

       public AsyncDelegateCommand(Func<TArgetType, Task> asyncExecute, Predicate<TArgetType> canExecute = null)
        {
            _asyncExecute = asyncExecute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute((TArgetType)parameter);
        }

        public async void Execute(object parameter)
        {
            await AsyncRunner(parameter);
        }

        public async Task ExecuteAsync(object parameter)
        {
            await AsyncRunner(parameter);
        }

        protected virtual async Task AsyncRunner(object parameter)
        {
            await _asyncExecute((TArgetType)parameter);
        }
    }
}