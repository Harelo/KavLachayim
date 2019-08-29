using KavLachayim.Helpers.MVVM;
using System.Collections.ObjectModel;
using static KavLachayim.Data.KavLachayimDB;

namespace KavLachayim.ViewModels
{
    /// <summary>
    /// A ViewModel for the ActivitiesPage
    /// </summary>
    public class ActivitiesPageViewModel : ViewModelBase
    {
        private ObservableCollection<ActivitiesTR> activitesCollection;
        /// <summary>
        /// Holds a collection of records from the ActivitiesPCT table
        /// </summary>
        public ObservableCollection<ActivitiesTR> ActivitiesCollection
        {
            set => SetProperty(ref activitesCollection, value);
            get => activitesCollection;
        }

        public ActivitiesPageViewModel()
        {
            async void GetActivities() => ActivitiesCollection = await App.Database.GetTableAsync<ActivitiesTR>();
            GetActivities();
        }
    }
}
