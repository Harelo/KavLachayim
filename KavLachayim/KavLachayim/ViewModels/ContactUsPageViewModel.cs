using static KavLachayim.Data.KavLachayimDB;
using System.Collections.ObjectModel;
using System.Linq;
using KavLachayim.Helpers.MVVM;

namespace KavLachayim.ViewModels
{
    class ContactUsPageViewModel : ViewModelBase
    {
        private ObservableCollection<ContactUsTR> contactUsPageCD;
        private string generalInfo;
        /// <summary>
        /// Holds a collection of records from the ContactUsPCT table
        /// </summary>
        public ObservableCollection<ContactUsTR> ContactUsPageCD
        {
            set => SetProperty(ref contactUsPageCD, value);
            get => contactUsPageCD;
        }

        /// <summary>
        /// A property that contains the general information section from the ContactUsPCT table
        /// </summary>
        public string GeneralInfo
        {
            set => SetProperty(ref generalInfo, value);
            get => generalInfo;
        }

        //The contructor for the ViewModel's class
        public ContactUsPageViewModel()
        {
            async void GetDetailsAsync()
            {
                var records = await App.Database.GetTableAsync<ContactUsTR>();
                ContactUsTR theGeneralRecord = records.Where(r => r.BranchName == "כללי").First();
                records.RemoveAt(theGeneralRecord.ID - 1);
                ContactUsPageCD = records;
                GeneralInfo = theGeneralRecord.OtherInfo;
            }

            GetDetailsAsync();
        }
    }
}
