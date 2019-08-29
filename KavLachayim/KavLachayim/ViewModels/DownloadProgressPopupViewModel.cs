using KavLachayim.Helpers.MVVM;
using KavLachayim.Views;
using System.ComponentModel;
using System.Net;
using Xamarin.Forms;

namespace KavLachayim.ViewModels
{
    public class DownloadProgressPopupViewModel : ViewModelBase
    {
        private double progress;
        /// <summary>
        /// Holds the current download progress
        /// </summary>
        public double Progress
        {
            set => SetProperty(ref progress, value);
            get => progress;
        }

        private string progressLabelText;
        /// <summary>
        /// The text to be presented in the ProgressLabel
        /// </summary>
        public string ProgressLabelText
        {
            set => SetProperty(ref progressLabelText, value);
            get => progressLabelText;
        }

        private bool finishedDownloading;
        /// <summary>
        /// Indicates whether the download has finished
        /// </summary>
        public bool FinishedDownloading
        {
            set => SetProperty(ref finishedDownloading, value);
            get => finishedDownloading;
        }

        /// <summary>
        /// Variable initalization
        /// </summary>
        public DownloadProgressPopupViewModel()
        {
            Progress = 0;
            FinishedDownloading = false;
            ProgressLabelText = "ההורדה מתחילה...";
            DownloadDB();
        }

        private async void DownloadDB()
        {
            var changedHandler = new DownloadProgressChangedEventHandler((s, e) =>
            {
                Progress = (double)e.ProgressPercentage / 100;
                ProgressLabelText = e.ProgressPercentage.ToString() + "% ירדו";

                if (e.ProgressPercentage == 100)
                {
                    ProgressLabelText = "ההורדה הסתיימה";
                    FinishedDownloading = true;
                }
            });

            await App.Database.UpdateDBFromServer(changedHandler).ConfigureAwait(false);
        }
    }
}
