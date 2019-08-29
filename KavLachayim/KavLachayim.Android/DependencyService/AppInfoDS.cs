using Android.Content;
using KavLachayim.DependencyInterfaces;
using KavLachayim.Droid.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppInfoDS))]
namespace KavLachayim.Droid.DependencyServices
{
    /// <summary>
    /// The AppInfo dependency service's android implementation. This class provides information about the application
    /// </summary>
    public class AppInfoDS : IAppInfoDS
    {
        public int Version
        {
            get
            {
                var contextWrapper = new ContextWrapper(Android.App.Application.Context);
                int currentVersionCode = contextWrapper.PackageManager.GetPackageInfo(contextWrapper.PackageName, 0).VersionCode;
                return currentVersionCode;
            }
        }
    }
}