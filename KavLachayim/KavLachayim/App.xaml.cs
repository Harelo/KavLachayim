using Xamarin.Forms;
using KavLachayim.Views;
using Plugin.Connectivity;
using KavLachayim.Data;
using KavLachayim.Helpers.MVVM;
using Rg.Plugins.Popup.Extensions;
using Plugin.DeviceInfo;
using Xamarin.Forms.Xaml;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace KavLachayim
{
    public partial class App : Application
    {
        /// <summary>
        /// Allows interaction with the database
        /// </summary>
        public static KavLachayimDB Database = KavLachayimDB.Database;

        /// <summary>
        /// A reference to the instance of the singleton ViewModelManager class
        /// </summary>
        public static ViewModelManager ViewModelManager = ViewModelManager.Manager;

        /// <summary>
        /// An instance of the MasterDetailPage is stored on this variable
        /// </summary>
        public static MasterDetailPage MasterPage { get; set; }

        /// <summary>
        /// An instance of the MainPage's navigation property
        /// </summary>
        public static INavigation MainNavigation { get; set; }

        /// <summary>
        /// Tells whether the user is or is not connected to the internet
        /// </summary>
        public static bool IsConnected => CrossConnectivity.Current.IsConnected;

        ///<summary>
        ///Stores the screen size of the device
        ///</summary>
        public static Size DeviceSize;

        public App()
        {
            InitializeComponent();
            MasterDetailPage mdPage = new MDPage();
            MainPage = new NavigationPage(mdPage);
            MasterPage = mdPage;
            DeviceSize = new Size(CrossDevice.Hardware.ScreenWidth, CrossDevice.Hardware.ScreenHeight);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            if (!IsConnected) MainPage.DisplayAlert("אינך מחובר", "תכנים מסוימים לא יהיו ניתנים להצגה היות ואינך מחובר לאינטרנט", "אישור");
            CheckForDatabaseUpdates();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        /// <summary>
        /// Checks if there's a database update and asks the user to allow an update if an update is found.
        /// If the user accepts the update, the new database will be downloaded.
        /// </summary>
        private async void CheckForDatabaseUpdates()
        {
            var updateFound = await Database.CheckDBUpdatesOnlineAsync();
            if (updateFound)
            {
                var userAcceptedUpdate = await MainPage.DisplayAlert("עדכון נמצא", "נמצא עדכון לאפליקציה - התוכן הנוכחי המצוי באפליקציה אינו מעודכן. האם לעדכן? (העידכון כרוך בשימוש באינטרנט)", "כן", "לא");
                if (userAcceptedUpdate)
                {
                    await MainPage.Navigation.PushPopupAsync(new DownloadProgressPopup());
                }
            }
        }
    }
}
