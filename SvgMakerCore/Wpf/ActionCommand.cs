using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SvgMakerCore.Wpf
{
    public class ActionCommnd : ICommand
    {
        public bool CanExecute(object parameter)
            => true;

        public void Execute(object parameter)
            => Action?.Invoke();

        public event EventHandler CanExecuteChanged;

        private Action Action { get; }

        public ActionCommnd(Action action)
            => Action = action;

        public void OnCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
