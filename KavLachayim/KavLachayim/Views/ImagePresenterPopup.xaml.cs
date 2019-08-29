using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms;

namespace KavLachayim.Views
{
    public partial class ImagePresenterPopup : PopupPage
    {
        /// <summary>
        /// Indicates whether the image is currently zoomed or not
        /// </summary>
        bool isZoomed = false;

        public ImagePresenterPopup(ImageSource imageSource)
        {
            InitializeComponent();
            PresentedImage.Source = imageSource;
        }

        /// <summary>
        /// Called when the image is tapped twice and performs zoom/unzoom operations on it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTapped(object sender, EventArgs e)
        {
            Animation animation;

            if (isZoomed == false)
            {
                animation = new Animation
                {
                   { 0, 1, new Animation(v => PresentedImage.WidthRequest = v, PresentedImage.Width, PresentedImage.Width * 2) },
                   { 0, 1, new Animation(v => PresentedImage.HeightRequest = v, PresentedImage.Height, PresentedImage.Height * 2) },
                };
            }

            else
                animation = new Animation
                {
                   { 0, 1, new Animation(v => PresentedImage.WidthRequest = v, PresentedImage.Width, PresentedImage.Width / 2) },
                   { 0, 1, new Animation(v => PresentedImage.HeightRequest = v, PresentedImage.Height, PresentedImage.Height / 2) }
                };


            animation.Commit(this, "zoomAnimation", easing: Easing.CubicInOut);
            isZoomed = !isZoomed;
        }
    }
}