using Xamarin.Forms;

namespace KavLachayim.Views
{
    public partial class ActivitiesPage : ContentPage
    {
        public ActivitiesPage()
        {
            InitializeComponent();
            ActivitiesContentLayout.ChildAdded += AddMarginToFirstChild;
        }

        //Set a space betwene the first child in ActivitiesContentLayout and its' parent
        private void AddMarginToFirstChild(object sender, ElementEventArgs e)
        {
            var view = e.Element as View;
            view.Margin = new Thickness(5, 5, 5, 0);
            ActivitiesContentLayout.ChildAdded -= AddMarginToFirstChild;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Set a space betwene the first child in ActivitiesContentLayout and its' parent
            if (ActivitiesContentLayout.Children != null && ActivitiesContentLayout.Children.Count > 1)
                AddMarginToFirstChild(this, new ElementEventArgs(ActivitiesContentLayout.Children[0]));
        }
    }
}