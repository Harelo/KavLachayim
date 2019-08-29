using System;
using System.Windows.Input;
using Xamarin.Forms;
using System.Reflection;
using KavLachayim.Helpers.Extensions;
using Rg.Plugins.Popup.Extensions;

namespace KavLachayim.Commands
{
    /// <summary>
    /// A command to return to the main page by popping the current page out of the page stack
    /// </summary>
    public class ClosePageCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private bool isBusy = false;

        public bool CanExecute(object parameter)
        {
            return !isBusy;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public async void Execute(object parameter)
        {
            isBusy = true;
            RaiseCanExecuteChanged();
            await App.MainNavigation.PopAsync(true);
            isBusy = false;
            RaiseCanExecuteChanged();
        }
    }

    /// <summary>
    /// A command to return to the main page by popping the current page out of the page stack
    /// </summary>
    public class ClosePopupCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private bool isBusy = false;

        public bool CanExecute(object parameter)
        {
            return !isBusy;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public async void Execute(object parameter)
        {
            isBusy = true;
            RaiseCanExecuteChanged();
            await App.MainNavigation.PopPopupAsync();
            isBusy = false;
            RaiseCanExecuteChanged();
        }
    }

    /// <summary>
    /// A command to open a page presenting a web page
    /// </summary>
    public class OpenWebPageCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private bool isBusy = false;

        public bool CanExecute(object parameter)
        {
            return !isBusy;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Execute(object url)
        {
            isBusy = true;
            RaiseCanExecuteChanged();
            Device.OpenUri(new Uri(url.ToString()));
            isBusy = false;
            RaiseCanExecuteChanged();
        }
    }

    /// <summary>
    /// A command to open a page
    /// </summary>
    public class OpenPageCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private bool isBusy = false;


        public bool CanExecute(object parameter) => !isBusy;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public async void Execute(object parameter)
        {
            var runningAssembly = typeof(OpenPageCommand).Assembly;
            var pageType = runningAssembly.GetTypeWithoutNamespace(parameter.ToString());

            isBusy = true;
            RaiseCanExecuteChanged();
            await App.MainNavigation.PushAsync((Page)Activator.CreateInstance(pageType), true);
            isBusy = false;
            RaiseCanExecuteChanged();
        }
    }
}
