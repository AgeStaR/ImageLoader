using System;
using System.Threading.Tasks;
using System.Windows;
using Prism.Commands;

namespace ImageLoader
{
    public static class SafeCommand
    {
        public static DelegateCommand Create(Func<Task> executeMethod, Func<bool> canExecuteMethod)
        {
            var safeAction = CreateSafeAction(executeMethod);

            return new DelegateCommand(safeAction, canExecuteMethod);
        }
        
        public static DelegateCommand Create(Func<Task> executeMethod)
        {
            var safeAction = CreateSafeAction(executeMethod);

            return new DelegateCommand(safeAction);
        }

        private static Action CreateSafeAction(Func<Task> action)
        {
            return async () =>
            {
                try
                {
                    await action();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            };
        }
    }
}