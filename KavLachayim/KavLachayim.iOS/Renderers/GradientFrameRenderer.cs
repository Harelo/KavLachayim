using CoreAnimation;
using CoreGraphics;
using KavLachayim.Controls;
using KavLachayim.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GradientFrame), typeof(GradientFrameRenderer))]
namespace KavLachayim.iOS.Renderers
{
    /// <summary>
    /// A renderer for the GradientFrame control
    /// </summary>
    public class GradientFrameRenderer : FrameRenderer
    {
        private Xamarin.Forms.Color StartColor { get; set; }
        private Xamarin.Forms.Color EndColor { get; set; }

        public override void Draw(CGRect rect)
        {
            CAGradientLayer gradient = new CAGradientLayer();
            gradient.Frame = NativeView.Bounds;
            gradient.NeedsDisplayOnBoundsChange = true;
            gradient.MasksToBounds = true;
            gradient.Colors = new CGColor[] { StartColor.ToCGColor(), EndColor.ToCGColor() };
            NativeView.Layer.InsertSublayer(gradient, 0);
            base.Draw(rect);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            var frame = e.NewElement as GradientFrame;
            StartColor = frame.StartColor;
            EndColor = frame.EndColor;
        }
    }
}



//This isn't currently working, should google it.
//Use this as help: https://forums.xamarin.com/discussion/90813/custom-render-for-label-with-gradient-background

//using CoreAnimation;
//using CoreGraphics;
//using UIKit;
//using Xamarin.Forms;
//using KavLachayim.Controls;
//using Xamarin.Forms.Platform.iOS;
//using KavLachayim.iOS.Renderers;

//[assembly: ExportRenderer(typeof(GradientFrame), typeof(GradientFrameRenderer))]
//namespace KavLachayim.iOS.Renderers
//{
//    public class GradientFrameRenderer : FrameRenderer
//    {
//        private GradientFrame _frame;

//        public override void Draw(CGRect rect)
//        {
//            var gradientView = new UIView(NativeView.Frame);
//            NativeView.BackgroundColor = UIColor.Clear;

//            var gradientLayer = new CAGradientLayer
//            {
//                Frame = gradientView.Layer.Bounds,
//                Colors = new CGColor[] { _frame.StartColor.ToCGColor(), _frame.EndColor.ToCGColor() }
//            };

//            gradientView.Layer.AddSublayer(gradientLayer);
//            gradientView.AddSubview(NativeView);

//            var gradientLabel = new UILabel(NativeView.Frame);
//            gradientLabel.AddSubview(gradientView);
//            SetNativeControl(gradientLabel);

//            if (_frame.Orientation == StackOrientation.Vertical)
//            {
//                gradientLayer.StartPoint = new CGPoint(0.5, 0);
//                gradientLayer.EndPoint = new CGPoint(0.5, 1.0);
//            }
//            else
//            {
//                gradientLayer.StartPoint = new CGPoint(0, 0.5);
//                gradientLayer.EndPoint = new CGPoint(1.0, 0.5);
//            }

//            base.Draw(rect);
//        }

//        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
//        {
//            base.OnElementChanged(e);

//            if (e.OldElement != null) return;
//            if (Equals(_frame, null))
//                _frame = e.NewElement as GradientFrame;
//        }
//    }
//}