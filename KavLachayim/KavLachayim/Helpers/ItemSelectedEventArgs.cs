using Xamarin.Forms;

namespace KavLachayim.Helpers
{
    public class ItemSelectedEventArgs
    {
        public int SelectedIndex { private set; get; }
        public View SelectedItem { private set; get; }

        public ItemSelectedEventArgs(int index, View view)
        {
            SelectedIndex = index;
            SelectedItem = view;
        }
    }
}
