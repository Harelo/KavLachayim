using Xamarin.Forms;

namespace KavLachayim.Views
{
    public partial class MDPageMaster : ContentPage
    {
        public ListView ListView;

        public MDPageMaster()
        {
            InitializeComponent();
            ListView = MenuItemsListView;
        }
    }
}