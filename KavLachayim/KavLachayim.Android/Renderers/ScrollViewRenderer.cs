using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using KavLachayim.Controls;
using Android.Content;

[assembly: ExportRenderer(typeof(KavLachayim.Controls.ScrollView), typeof(KavLachayim.Droid.Renderers.ScrollViewRenderer))]
namespace KavLachayim.Droid.Renderers
{
    public class ScrollViewRenderer : Xamarin.Forms.Platform.Android.ScrollViewRenderer
    {
        private Controls.ScrollView scrollView;

        public ScrollViewRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null) e.OldElement.PropertyChanged -= ScrollViewPropertyChanged;
            if (e.OldElement != null || Element == null) return;

            scrollView = e.NewElement as KavLachayim.Controls.ScrollView;
            ChildViewAdded += OnChildViewAdded;
            e.NewElement.PropertyChanged += ScrollViewPropertyChanged;
        }

        private void OnChildViewAdded(object sender, ChildViewAddedEventArgs e)
        {
            SetScrollbars();
            ChildViewAdded -= OnChildViewAdded;
        }

        private void SetScrollbars()
        {
            switch (scrollView.ScrollBars)
            {
                case Controls.ScrollView.ScrollBarsOptions.Horizontal:
                    VerticalScrollBarEnabled = false;
                    break;
                case Controls.ScrollView.ScrollBarsOptions.Vertical:
                    GetChildAt(0).HorizontalScrollBarEnabled = false;
                    break;
                case Controls.ScrollView.ScrollBarsOptions.None:
                    GetChildAt(0).HorizontalScrollBarEnabled = false;
                    VerticalScrollBarEnabled = false;
                    break;
            }
        }

        private void ScrollViewPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ScrollBars":
                    SetScrollbars();
                    break;
            }
        }
    }
}