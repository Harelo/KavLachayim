using Foundation;
using KavLachayim.DependencyInterfaces;
using KavLachayim.iOS.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppInfoDS))]
namespace KavLachayim.iOS.DependencyServices
{
    public class AppInfoDS : IAppInfoDS
    {
        public int Version => int.Parse(NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString());
    }
}