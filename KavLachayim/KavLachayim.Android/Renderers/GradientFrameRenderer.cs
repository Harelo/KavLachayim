using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using KavLachayim.Controls;
using KavLachayim.Droid.Renderers;
using Android.Graphics;
using Android.Content;

[assembly: ExportRenderer(typeof(GradientFrame), typeof(GradientFrameRenderer))]
namespace KavLachayim.Droid.Renderers
{
    /// <summary>
    /// A renderer for the GradientFrame control
    /// </summary>
    public class GradientFrameRenderer : FrameRenderer
    {
        private Xamarin.Forms.Color StartColor { get; set; }
        private Xamarin.Forms.Color EndColor { get; set; }

        public GradientFrameRenderer(Context context) : base(context) { }

        protected override void DispatchDraw(Canvas canvas)
        {
            Button button = new Button();
            var gradient = new LinearGradient(0, 0, Width, Height, StartColor.ToAndroid(), EndColor.ToAndroid(), Shader.TileMode.Mirror);
            var paint = new Paint()
            {
                Dither = true,
            };
            paint.SetShader(gradient);
            canvas.DrawPaint(paint);
            base.DispatchDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null) return;

            var page = e.NewElement as GradientFrame;
            StartColor = page.StartColor;
            EndColor = page.EndColor;
        }
    }
}