using System.Collections.ObjectModel;
using static KavLachayim.Data.KavLachayimDB;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;
using KavLachayim.Views;
using System.IO;
using KavLachayim.Helpers.Extensions;
using KavLachayim.Helpers.MVVM;
using KavLachayim.Helpers;
using System.Linq;

namespace KavLachayim.ViewModels
{
    /// <summary>
    /// A ViewModel for the ArticlesAboutUs page
    /// </summary>
    public class ArticlesAboutUsViewModel : ViewModelBase
    {
        private ObservableCollection<ArticlesAboutUsTR> articlesCollection;
        /// <summary>
        /// Holds a collection of records from the ArticlesAboutUsPCT table
        /// </summary>
        public ObservableCollection<ArticlesAboutUsTR> ArticlesCollection
        {
            set => SetProperty(ref articlesCollection, value);
            get => articlesCollection;
        }

        private ArticlesAboutUsTR selectedArticle;
        /// <summary>
        /// Bound to the SelectedItem property of the Articles ListView
        /// </summary>
        public ArticlesAboutUsTR SelectedArticle
        {
            set
            {
                if (value == null) return;

                SetProperty(ref selectedArticle, value);

                OpenArticle();
            }

            get => selectedArticle;
        }

        /// <summary>
        /// Opens an article asynchronically
        /// </summary>
        private async void OpenArticle()
        {
            if (SelectedArticle.UrlAddress != null)
                Device.OpenUri(new System.Uri(SelectedArticle.UrlAddress));

            else
            {
                var imageBytes = SelectedArticle.Image;
                var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                await App.MainNavigation.PushPopupAsync(new ImagePresenterPopup(imageSource));
            }
        }

        /// <summary>
        /// The contructor for the ViewModel's class. Fills the ObservableCollection with data
        /// </summary>
        public ArticlesAboutUsViewModel()
        {
            async void GetArticles() => ArticlesCollection = await App.Database.GetTableAsync<ArticlesAboutUsTR>();
            GetArticles();
        }
    }
}
