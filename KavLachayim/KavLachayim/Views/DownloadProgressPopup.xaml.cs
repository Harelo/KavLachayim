using Rg.Plugins.Popup.Pages;

namespace KavLachayim.Views
{
    public partial class DownloadProgressPopup : PopupPage
    {
        public DownloadProgressPopup()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed() => !CloseLabel.IsVisible;
    }
}