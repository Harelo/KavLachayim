using KavLachayim.Helpers.MVVM;
using System.Collections.ObjectModel;
using static KavLachayim.Data.KavLachayimDB;

namespace KavLachayim.ViewModels
{
    public class ThanksPageViewModel : ViewModelBase
    {
        private ObservableCollection<ThanksTR> imagesCollection;
        /// <summary>
        /// Holds the collection of images for the Thanks page
        /// </summary>
        public ObservableCollection<ThanksTR> ImagesCollection
        {
            set => SetProperty(ref imagesCollection, value);
            get => imagesCollection;
        }

        public ThanksPageViewModel()
        {
            async void GetImages() => ImagesCollection = await App.Database.GetTableAsync<ThanksTR>();
            GetImages();
        }
    }
}
