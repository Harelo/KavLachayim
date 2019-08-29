using KavLachayim.Helpers.Extensions;
using static KavLachayim.Data.KavLachayimDB;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using KavLachayim.Helpers.MVVM;
using System;

namespace KavLachayim.ViewModels
{
    /// <summary>
    /// A ViewModel for the donation options page
    /// </summary>
    public class DonateTabbedPageViewModel : ViewModelBase
    {
        private ObservableCollection<DonationOptionsTR> donationOptionsData;
        /// <summary>
        /// Holds a collection of records from the DonationOptionsPCT table
        /// </summary>
        public ObservableCollection<DonationOptionsTR> DonationOptionsData
        {
            set => SetProperty(ref donationOptionsData, value);
            get => donationOptionsData;
        }

        private ObservableCollection<string> donateToOptions;
        /// <summary>
        /// Holds a collection of all the campaigns the user can donate to
        /// </summary>
        public ObservableCollection<string> DonateToOptions
        {
            set => SetProperty(ref donateToOptions, value);
            get => donateToOptions;
        }

        //Variables used for binding purposes to get the user's desired currency, donation amount and campaign

        private int donationCurrency;
        public int DonationCurrency
        {
            set => SetProperty(ref donationCurrency, value);
            get => donationCurrency;
        }

        private int donationAmount;
        public int DonationAmount
        {
            set => SetProperty(ref donationAmount, value);
            get => donationAmount;
        }

        private string campaignTitle;
        public string CampaignTitle
        {
            set
            {
                SetProperty(ref campaignTitle, value);

                if (DonationOptionsData != null && campaignTitle != null)
                    UpdateOptions(campaignTitle);
            }
            get => campaignTitle;
        }

        private int chosenDonateToIndex;
        public int ChosenDonateToIndex
        {
            set => SetProperty(ref chosenDonateToIndex, value);
            get => chosenDonateToIndex;
        }

        private Command openDonatePageCommand;
        /// <summary>
        /// A command to open the donations page
        /// </summary>
        public Command OpenDonatePageCommand
        {
            get
            {
                if (openDonatePageCommand == null)
                    openDonatePageCommand = new Command(OpenDonatePageExecute, p => !IsBusy);
                return openDonatePageCommand;
            }
        }

        /// <summary>
        /// A command to open the donations page according to a given donation amount and currency
        /// </summary>
        /// <param name="parameter"></param>
        private async void OpenDonatePageExecute(object parameter)
        {
            string currency = "";
            switch (DonationCurrency)
            {
                case 0:
                    currency = "ILS";
                    break;
                case 1:
                    currency = "USD";
                    break;
                default:
                    currency = "ILS";
                    break;
            }

            var forQueryValue = await App.Database.ExecuteScalarAsync<string>($@"SELECT ForQueryValue FROM CampaignsPCT WHERE Title=""{CampaignTitle}""");

            if (CampaignTitle == "קו לחיים" || forQueryValue == null || forQueryValue == "")
                forQueryValue = CampaignTitle.Replace(' ', '+');

            string url = $"http://www.xsc.co.il/kav/createTransaction.php?amount={DonationAmount.ToString()}&for={forQueryValue}&currency={currency.ToString()}";
            IsBusy = true;
            OpenDonatePageCommand.ChangeCanExecute();
            Device.OpenUri(new Uri(url));
            IsBusy = false;
            OpenDonatePageCommand.ChangeCanExecute();
        }

        /// <summary>
        /// The contructor for the ViewModel's class. Fills the ObservableCollection with data
        /// </summary>
        public DonateTabbedPageViewModel()
        {
            async void GetCollections()
            {
                DonationOptionsData = await App.Database.GetTableAsync<DonationOptionsTR>();
                DonateToOptions = await App.Database.GetDonateToOptionsAsync();
                OnPropertyChanged(nameof(ChosenDonateToIndex));
            }

            GetCollections();
        }

        private async void UpdateOptions(string title)
        {
            DonationOptionsData = await App.Database.GetTableAsync<DonationOptionsTR>();

            if (CampaignTitle != "קו לחיים")
            {
                DonationOptionsData[1].Content = DonationOptionsData[1].Content.Replace("קו לחיים", @"ע""ש קו לחיים עבור " + title);
                DonationOptionsData[2].Content = DonationOptionsData[2].Content.Replace("קו לחיים", "קו לחיים עבור " + title);
            }

            OnPropertyChanged(nameof(DonationOptionsData));
        }
    }
}