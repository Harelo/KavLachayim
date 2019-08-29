using Xamarin.Forms;

namespace KavLachayim.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            AboutContentLayout.ChildAdded += AddMarginToFirstChild;
        }

        //Set a space betwene the first child in AboutContentLayout and its' parent
        private void AddMarginToFirstChild(object sender, ElementEventArgs e)
        {
            var view = e.Element as View;
            view.Margin = new Thickness(5, 5, 5, 0);
            AboutContentLayout.ChildAdded -= AddMarginToFirstChild;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Set a space betwene the first child in AboutContentLayout and its' parent
            if (AboutContentLayout.Children != null && AboutContentLayout.Children.Count > 1)
                AddMarginToFirstChild(this, new ElementEventArgs(AboutContentLayout.Children[0]));
        }
    }
}