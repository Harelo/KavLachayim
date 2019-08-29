using KavLachayim.Helpers.MVVM;
using System.Collections.ObjectModel;
using static KavLachayim.Data.KavLachayimDB;

namespace KavLachayim.ViewModels
{
    /// <summary>
    /// A ViewModel for the Employees PopupPage
    /// </summary>
    public class EmployeesPopupViewModel : ViewModelBase
    {
        private ObservableCollection<EmployeesTR> employeesCollection;

        /// <summary>
        /// Contains all the employees information for the employees popup
        /// </summary>
        public ObservableCollection<EmployeesTR> EmployeesCollection
        {
            set => SetProperty(ref employeesCollection, value);
            get => employeesCollection;
        }

        public EmployeesPopupViewModel()
        {
            //Fill the collection of employees with data from the database
            async void GetEmployees() => EmployeesCollection = await App.Database.GetTableAsync<EmployeesTR>();
            GetEmployees();
        }
    }
}
