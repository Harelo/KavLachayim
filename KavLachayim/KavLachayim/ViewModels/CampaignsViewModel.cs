using System.Collections.ObjectModel;
using static KavLachayim.Data.KavLachayimDB;
using KavLachayim.Views;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using System.IO;
using KavLachayim.Helpers.MVVM;
using System.Linq;
using KavLachayim.Helpers;

namespace KavLachayim.ViewModels
{
    /// <summary>
    /// A ViewModel for the Campaigns page and the campaigns information page
    /// </summary>
    public class CampaignsViewModel : ViewModelBase
    {
        private ObservableCollection<CampaignsTR> campaignsCollection;
        /// <summary>
        /// Holds a collection of records from the CampaignsPCT table
        /// </summary>
        public ObservableCollection<CampaignsTR> CampaignsCollection
        {
            set => SetProperty(ref campaignsCollection, value);
            get => campaignsCollection;
        }

        /// <summary>
        /// The selected campaign. Used to present a campaign selected by the user.
        /// </summary>
        public CampaignsTR SelectedCampaign { get; set; }

        private Command donateCommand;
        /// <summary>
        /// A command to open the donation page for the chosen campaign
        /// </summary>
        public Command DonateCommand
        {
            get
            {
                if (donateCommand == null)
                    donateCommand = new Command(async (p) =>
                    {
                        IsBusy = true;
                        donateCommand.ChangeCanExecute();
                        App.MasterPage.Detail = new DonateTabbedPage();
                        App.MasterPage.Title = "אפשרויות לתרומה";
                        await App.MainNavigation.PopAllPopupAsync();
                        await App.MainNavigation.PopToRootAsync();

                        if (App.ViewModelManager.Get(typeof(DonateTabbedPageViewModel)) == null)
                            App.ViewModelManager.Add(typeof(DonateTabbedPageViewModel));

                        //Get the index of the chosen campaign in the DonateToOptions array
                        var viewModel = (DonateTabbedPageViewModel)App.ViewModelManager.Get(typeof(DonateTabbedPageViewModel));
                        var chosenID = viewModel.DonateToOptions.IndexOf(SelectedCampaign.Title);
                        viewModel.ChosenDonateToIndex = chosenID;

                        IsBusy = false;
                        donateCommand.ChangeCanExecute();
                    }, p => !IsBusy);

                return donateCommand;
            }
        }

        private Command viewImage;
        /// <summary>
        /// A command to view an image
        /// </summary>
        public Command ViewImage
        {
            get
            {
                if (viewImage == null)
                    viewImage = new Command(async (p) =>
                    {
                        IsBusy = true;
                        ViewImage.ChangeCanExecute();
                        var imageBytes = SelectedCampaign.ContentImage;
                        var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                        await App.MainNavigation.PushPopupAsync(new ImagePresenterPopup(imageSource));
                        IsBusy = false;
                        ViewImage.ChangeCanExecute();
                    }, p => !IsBusy);

                return viewImage;
            }
        }

        private Command viewCampaignCommand;
        /// <summary>
        /// A command to view a campaign by opening a popup with the campaign's details
        /// </summary>
        public Command ViewCampaignCommand
        {
            get
            {
                if (viewCampaignCommand == null)
                    viewCampaignCommand = new Command(async (p) =>
                    {
                        IsBusy = true;
                        viewCampaignCommand.ChangeCanExecute();
                        var selectedIndex = ((ItemSelectedEventArgs)p).SelectedIndex;
                        SelectedCampaign = CampaignsCollection[selectedIndex];
                        await App.MainNavigation.PushPopupAsync(new CampaignPopup());
                        IsBusy = false;
                        viewCampaignCommand.ChangeCanExecute();
                        donateCommand.ChangeCanExecute();
                    }, p => !IsBusy);

                return viewCampaignCommand;
            }
        }

        //The contructor for the ViewModel's class. Fills the ObservableCollection with data
        public CampaignsViewModel()
        {
            async void GetCampaigns() => CampaignsCollection = await App.Database.GetTableAsync<CampaignsTR>();
            GetCampaigns();
        }
    }

}
