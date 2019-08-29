using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using System.Threading;
using Android.Content.PM;
using KavLachayim.Droid.Activites;

namespace KavLachayim.Droid.Activities
{
    //Theme = "@style/Theme.Splash"

    /// <summary>
    /// An activity used to present a splash screen
    /// </summary>
    [Activity(MainLauncher = true, NoHistory = true, Theme = "@style/Theme.Splash", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : Activity
    {
        /// <summary>
        /// Called when the activity is first created, in this case, when the application opens
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Store a color defined in Resources/colors.xml in a variable
            Color KavLachayimColor = new Color(ContextCompat.GetColor(this, Resource.Color.kavlachayim));
            //Store an image stored in Resources/drawable in a variable of type Bitmap
            Bitmap bm = BitmapFactory.DecodeResource(Resources, Resource.Drawable.logo_white);
            //Change the properties of the TaskDescription for the application (what's shown when you see the application in
            //the recents screen / "task manager")

            ActivityManager.TaskDescription td = new ActivityManager.TaskDescription("קו לחיים", bm, KavLachayimColor);
            SetTaskDescription(td);

            //Start the MainActivity activity
            StartActivity(typeof(MainActivity));
        }
    }
}