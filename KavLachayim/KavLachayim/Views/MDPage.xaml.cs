using System;
using KavLachayim.Models;
using Xamarin.Forms;
using System.Linq;

namespace KavLachayim.Views
{
    public partial class MDPage : MasterDetailPage
    {
        public MDPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var Item = e.SelectedItem as MDPageMenuItemModel;
            if (Item == null)
                return;

            var page = (Page)Activator.CreateInstance(Item.TargetType);
            Detail = new NavigationPage(page);
            NavigationPage.SetHasNavigationBar(page, false);
            Title = Item.Title;
            IsPresented = false;
            MasterPage.ListView.SelectedItem = null;
        }


        protected override void OnAppearing()
        {
            App.MainNavigation = Navigation;
            base.OnAppearing();
        }
    }
}