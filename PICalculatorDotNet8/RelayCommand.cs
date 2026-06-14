using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PICalculatorDotNet8
{
    class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        Action callback;
        Func<bool> canExcute;

        public RelayCommand(Action callback, Func<bool> canExcute = null)
        {
            this.callback = callback;
            this.canExcute = canExcute;
        }

        public bool CanExecute(object parameter)
        {
            if (this.canExcute == null) return false;
            return this.canExcute();
        }

        public void Execute(object parameter)
        {
            this.callback?.Invoke();
        }
    }

    internal class RelayCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        Action<T> callback;
        Func<bool, T> canExcute;

        public RelayCommand(Action<T> callback, Func<bool, T> canExcute = null)
        {
            this.callback = callback;
            this.canExcute = canExcute;
        }

        public bool CanExecute(object parameter)
        {
            if (this.canExcute == null) return false;
            return this.CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.callback?.Invoke((T)parameter);
        }
    }
}
