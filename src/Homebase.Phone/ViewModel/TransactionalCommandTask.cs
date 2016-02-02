using GalaSoft.MvvmLight.Ioc;
using Homebase.Business.Data.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Homebase.Phone.ViewModel
{
    public class TransactionalCommandTask : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public bool IsExecuting { get; protected set; }

        private readonly Func<bool> _canExecute;
        private readonly Func<Task> _executeAsync;

        protected readonly IDataContextManager _dataContextManager;

        protected TransactionalCommandTask(Func<bool> canExecute)
        {
            if (canExecute == null) throw new ArgumentNullException(nameof(canExecute));

            _canExecute = canExecute;
            _dataContextManager = SimpleIoc.Default.GetInstance<IDataContextManager>();
        }

        public TransactionalCommandTask(Func<Task> execute)
            : this(execute, () => true)
        {
        }

        public TransactionalCommandTask(Func<Task> execute, Func<bool> canExecute)
            : this(canExecute)
        {
            if (execute == null) throw new ArgumentNullException(nameof(execute));

            _executeAsync = execute;
        }

        public bool CanExecute(object parameter)
        {
            return !IsExecuting && _canExecute();
        }

        [Obsolete("Do not call directly")]
        public async void Execute(object parameter)
        {
            await Execute();
        }

        public async Task Execute()
        {
            try
            {
                IsExecuting = true;
                RaiseCanExecuteChanged();

                await _dataContextManager.BeginTransaction();

                await _executeAsync();

                await _dataContextManager.SaveChanges();
            }
            catch (Exception)
            {
                await _dataContextManager.Rollback();

                throw;
            }
            finally
            {
                IsExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
