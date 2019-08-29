
namespace KavLachayim.Droid.DependencyService
{
    public class ScreenSizeHelper
    {
        public void GetScreenSize()
        {
            //Get the screen size of the screen
            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
        }
    }
}