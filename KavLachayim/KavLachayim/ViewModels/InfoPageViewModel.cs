using System.Collections.ObjectModel;
using static KavLachayim.Data.KavLachayimDB;
using KavLachayim.Helpers.MVVM;

namespace KavLachayim.ViewModels
{
    /// <summary>
    /// A ViewModel for the information page
    /// </summary>
    public class InfoPageViewModel : ViewModelBase
    {
        private ObservableCollection<InfoPageContentTR> contentsCollection;
        public ObservableCollection<InfoPageContentTR> ContentsCollection
        {
            set => SetProperty(ref contentsCollection, value);
            get => contentsCollection;
        }

        private string pageTitle;
        public string PageTitle
        {
            set => SetProperty(ref pageTitle, value);
            get => pageTitle;
        }

        private int cvPosition;
        public int CVPosition
        {
            set
            {
                SetProperty(ref cvPosition, value);

                if (ContentsCollection != null)
                    PageTitle = ContentsCollection[value].Title;
            }
            get => cvPosition;
        }

        public InfoPageViewModel()
        {
            async void FillContents()
            {
                ContentsCollection = await App.Database.GetTableAsync<InfoPageContentTR>();
                PageTitle = ContentsCollection[CVPosition].Title;
            }
            FillContents();
        }
    }
}
