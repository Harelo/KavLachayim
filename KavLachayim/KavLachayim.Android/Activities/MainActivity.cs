using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using CarouselView.FormsPlugin.Android;

namespace KavLachayim.Droid.Activites
{
    /// <summary>
    /// The main activity of the Android application
    /// </summary>
    [Activity(Label = "KavLachayim", Icon = "@drawable/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            //Store a color defined in Resources/colors.xml in a variable
            Android.Graphics.Color KavLachayimColor = new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.kavlachayim));
            //Store an image stored in Resources/drawable in a variable of type Bitmap
            Bitmap bm = BitmapFactory.DecodeResource(Resources, Resource.Drawable.logo_white);
            //Change the properties of the TaskDescription for the application (what's shown when you see the application in
            //the recents screen / "task manager")

            ActivityManager.TaskDescription td = new ActivityManager.TaskDescription("קו לחיים", bm, KavLachayimColor);
            SetTaskDescription(td);

            Forms.Init(this, bundle);
            CarouselViewRenderer.Init(); //Init CarouselViewRenderer
            LoadApplication(new App());
        }
    }
}

