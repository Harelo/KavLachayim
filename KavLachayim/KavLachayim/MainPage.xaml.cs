using KavLachayim.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KavLachayim
{
    /// <summary>
    /// The main page of the app
    /// </summary>
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            WV.Navigated += (s, e) => Loading.IsRunning = false;
            WV.Navigating += (s, e) => { if (!Loading.IsRunning) e.Cancel = true; };
        }

        //Gets called when the page appears
        protected override void OnAppearing()
        {
            MessagingCenter.Send(this, "Appeared");
            base.OnAppearing();
        }
    }
}
