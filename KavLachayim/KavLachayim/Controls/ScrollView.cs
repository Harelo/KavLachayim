using Xamarin.Forms;

namespace KavLachayim.Controls
{
    public class ScrollView : Xamarin.Forms.ScrollView
    {
        public enum ScrollBarsOptions { Both, Horizontal, Vertical, None }

        public static readonly BindableProperty ScrollBarsProperty = BindableProperty.Create(
            "ScrollBars",
            typeof(ScrollBarsOptions),
            typeof(ScrollView),
            ScrollBarsOptions.Both);

        public ScrollBarsOptions ScrollBars
        {
            set => SetValue(ScrollBarsProperty, value);
            get => (ScrollBarsOptions)GetValue(ScrollBarsProperty);
        }
    }
}
