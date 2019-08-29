using Xamarin.Forms;

namespace KavLachayim.Views
{
    public partial class ArticlesAboutUs : ContentPage
    {
        public ArticlesAboutUs()
        {
            InitializeComponent();
        }

        private void ClearArticlesListViewSelectedItem(object sender, SelectedItemChangedEventArgs e) => ArticlesListView.SelectedItem = null;
    }
}