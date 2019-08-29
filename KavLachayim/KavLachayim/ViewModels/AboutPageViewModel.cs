using KavLachayim.Commands;
using KavLachayim.Helpers;
using System.Collections.ObjectModel;
using static KavLachayim.Data.KavLachayimDB;
using Rg.Plugins.Popup.Extensions;
using KavLachayim.Views;
using Xamarin.Forms;
using KavLachayim.Helpers.MVVM;

namespace KavLachayim.ViewModels
{
    /// <summary>
    /// A ViewModel for the About page
    /// </summary>
    public class AboutPageViewModel : ViewModelBase
    {
        private ObservableCollection<AboutTR> aboutUsInfoCollection;
        /// <summary>
        /// Holds a collection of records from the AboutUsPCT table
        /// </summary>
        public ObservableCollection<AboutTR> AboutUsInfoCollection
        {
            set => SetProperty(ref aboutUsInfoCollection, value);
            get => aboutUsInfoCollection;
        }

        private Command openEmployeesListCommand;
        /// <summary>
        /// A command used to open the employees list as a popup
        /// </summary>
        public Command OpenEmployeesListCommand
        {
            get
            {
                if (openEmployeesListCommand == null)
                    openEmployeesListCommand = new Command(async (p) =>
                    {
                        IsBusy = true;
                        OpenEmployeesListCommand.ChangeCanExecute();
                        await App.MainNavigation.PushPopupAsync(new EmployeesPopup());
                        IsBusy = false;
                        OpenEmployeesListCommand.ChangeCanExecute();
                    }, p => !IsBusy);
                return openEmployeesListCommand;
            }
        }

        //The contructor for the ViewModel's class. Fills the ObservableCollection with data
        public AboutPageViewModel()
        {
            async void GetContents() => AboutUsInfoCollection = await App.Database.GetTableAsync<AboutTR>();
            GetContents();
        }
    }
}
