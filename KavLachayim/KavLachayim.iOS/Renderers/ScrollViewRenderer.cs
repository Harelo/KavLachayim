using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(KavLachayim.Controls.ScrollView), typeof(KavLachayim.iOS.Renderers.ScrollViewRenderer))]
namespace KavLachayim.iOS.Renderers
{
    public class ScrollViewRenderer : Xamarin.Forms.Platform.iOS.ScrollViewRenderer
    {
        private Controls.ScrollView scrollView;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null) e.OldElement.PropertyChanged -= ScrollViewPropertyChanged;
            if (e.OldElement != null || Element == null) return;

            scrollView = e.NewElement as KavLachayim.Controls.ScrollView;
            SetScrollbars();
            e.NewElement.PropertyChanged += ScrollViewPropertyChanged;
        }

        private void SetScrollbars()
        {
            switch (scrollView.ScrollBars)
            {
                case Controls.ScrollView.ScrollBarsOptions.Horizontal:
                    ShowsVerticalScrollIndicator = false;
                    break;
                case Controls.ScrollView.ScrollBarsOptions.Vertical:
                    ShowsHorizontalScrollIndicator = false;
                    break;
                case Controls.ScrollView.ScrollBarsOptions.None:
                    ShowsVerticalScrollIndicator = false;
                    ShowsHorizontalScrollIndicator = false;
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