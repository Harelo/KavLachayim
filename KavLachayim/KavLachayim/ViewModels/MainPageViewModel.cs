using Xamarin.Forms;
using KavLachayim.Models;
using System.Collections.ObjectModel;
using System.Timers;
using KavLachayim.Views;
using KavLachayim.Helpers.MVVM;

namespace KavLachayim.ViewModels
{
    /// <summary>
    /// A ViewModel for the main page
    /// </summary>
    public class MainPageViewModel : ViewModelBase
    {
        private ObservableCollection<MainPageCarouselModel> mainPageCD;
        public ObservableCollection<MainPageCarouselModel> MainPageCD
        {
            set => SetProperty(ref mainPageCD, value);
            get => mainPageCD;
        }

        /// <summary>
        /// A timer to be used to scroll between items in the CarouselView on the MainPage
        /// </summary>
        public Timer Timer;

        private int cvPosition;
        /// <summary>
        /// Holds the current position of the CarouselView in the MainPage
        /// </summary>
        public int CVPosition
        {
            set => SetProperty(ref cvPosition, value);
            get => cvPosition;
        }

        /// <summary>
        /// Constructor of the ViewModel
        /// </summary>
        public MainPageViewModel()
        {
            MessagingCenter.Subscribe<MainPage>(this, "Appeared", (p) =>
            {
                CVPosition = 0;
                if (Timer == null || !Timer.Enabled)
                    StartScrollingTimer();
            });

            MainPageCD = new ObservableCollection<MainPageCarouselModel>(new[]
            {
                new MainPageCarouselModel() {ImageSource="info1.jpg", OnClickParameter="שינוי חברתי"},
                new MainPageCarouselModel() {ImageSource="info2.jpg", OnClickParameter="שיפור חיים" },
                new MainPageCarouselModel() {ImageSource="info3.jpg", OnClickParameter="הצלת חיים"}
            });
        }

        private Command openDetailPageCommand;
        public Command OpenDetailPageCommand
        {
            get
            {
                if (openDetailPageCommand == null)
                    openDetailPageCommand = new Command(OpenDetailPageExecute, p => true);
                return openDetailPageCommand;
            }
        }

        private void OpenDetailPageExecute(object parameter)
        {
            var pageName = parameter.ToString();
            switch (pageName)
            {
                case "DonateTabbedPage":    
                    App.MasterPage.Detail = new DonateTabbedPage();
                    App.MasterPage.Title = "אפשרויות לתרומה";
                    break;

                case "CampaignsPage":
                    App.MasterPage.Detail = new CampaignsPage();
                    App.MasterPage.Title = "קמפיינים מצילי חיים";
                    break;
            }
        }


        private Command infoImgTapCommand;
        /// <summary>
        /// A command which is executed when an info image is tapped
        /// </summary>
        public Command InfoImgTapCommand
        {
            get
            {
                if (infoImgTapCommand == null)
                    infoImgTapCommand = new Command(InfoImgTapExecute, p => !IsBusy);
                return infoImgTapCommand;
            }
        }

        public async void InfoImgTapExecute(object parameter)
        {
            var infoPageViewModel = App.ViewModelManager.Get(typeof(InfoPageViewModel));
            if (infoPageViewModel == null)
                App.ViewModelManager.Add(typeof(InfoPageViewModel));
            ((InfoPageViewModel)App.ViewModelManager.Get(typeof(InfoPageViewModel))).CVPosition = CVPosition;

            Timer.Stop();
            IsBusy = true;
            InfoImgTapCommand.ChangeCanExecute();
            await App.MainNavigation.PushAsync(new InfoPage(), true);
            IsBusy = false;
            InfoImgTapCommand.ChangeCanExecute();
        }

        /// <summary>
        /// Starts the info CarouselView scrolling timer
        /// </summary>
        public void StartScrollingTimer()
        {
            CVPosition = 0;
            Timer = new Timer(10000);
            Timer.Elapsed += (s, e) => CVPosition = (CVPosition == MainPageCD.Count - 1) ? 0 : CVPosition + 1;
            Timer.AutoReset = true;
            Timer.Start();
        }
    }
}