using Xamarin.Forms;

namespace KavLachayim.Controls
{
    /// <summary>
    /// A frame with gradient background
    /// </summary>
    public class GradientFrame : Frame
    {
        /// <summary>
        /// The start color of the frame. Default is transparent
        /// </summary>
        public Color StartColor { get; set; } = Color.Transparent;
        /// <summary>
        /// The end color of the frame.  Default is transparent
        /// </summary>
        public Color EndColor { get; set; } = Color.Transparent;

        public GradientFrame()
        {
            OutlineColor = Color.Transparent;
        }
    }
}
