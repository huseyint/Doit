using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Doit.Infrastructure
{
	public class DelegateCommand : ICommand
	{
		private readonly Dispatcher _dispatcher;

		private readonly Predicate<object> _canExecute;
		
		private readonly Action<object> _execute;

		public DelegateCommand(Action<object> execute, Predicate<object> canExecute = null)
		{
			_dispatcher = Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;

			Debug.Assert(_dispatcher != null, "Can't find dispatcher");

			_execute = execute;
			_canExecute = canExecute;
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public bool CanExecute(object parameter)
		{
			if (_canExecute == null)
			{
				return true;
			}

			return _canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			_execute(parameter);
		}

		protected virtual void OnCanExecuteChanged()
		{
			if (!_dispatcher.CheckAccess())
			{
				_dispatcher.Invoke((ThreadStart)OnCanExecuteChanged, DispatcherPriority.Normal);
			}
			else
			{
				CommandManager.InvalidateRequerySuggested();
			}
		}
	}
}