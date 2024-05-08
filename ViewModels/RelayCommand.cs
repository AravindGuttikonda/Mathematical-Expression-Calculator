using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF_Expression_Calculator.ViewModels
{
    public class RelayCommand : ICommand
    {
        public Action OnExecute;
        
        public RelayCommand(Action OnExecute)
        {
            this.OnExecute = OnExecute;
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            OnExecute();
        }
    }
}
