using Xamarin.Forms;

namespace KavLachayim.Views
{
    public partial class CertificatesPage : ContentPage
    {
        public CertificatesPage()
        {
            InitializeComponent();
        }

        private void ClearFormsListViewSelectedItem(object sender, SelectedItemChangedEventArgs e)
            => FormsListView.SelectedItem = null;

        private void ClearApprovalFormsListViewSelectedItem(object sender, SelectedItemChangedEventArgs e)
            => ApprovalFormsListView.SelectedItem = null;
    }
}