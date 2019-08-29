using KavLachayim.DependencyInterfaces;
using Xamarin.Forms;
using KavLachayim.Droid.DependencyServices;
using System.IO;
using System;
using Android.Preferences;
using Android.Content;

[assembly: Dependency(typeof(DatabaseDS))]
namespace KavLachayim.Droid.DependencyServices
{
    /// <summary>
    /// The Database dependency service's android implementation. This class is used to perform operations on the database.
    /// </summary>
    public class DatabaseDS : IDatabaseDS
    {
        private string fullDBPath, DBName;

        /// <summary>
        /// Used to get the path to the database
        /// </summary>
        /// <param name="dbName">The name of the database to find</param>
        /// <returns>The database path</returns>
        public string GetDatabasePath(string dbName)
        {
            string DBFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            DBName = dbName;
            fullDBPath = Path.Combine(DBFolderPath, DBName);

            //  Checks the current version code, if it's higher then the previous stored one, save the new one
            //  and update the database
            var prefs = PreferenceManager.GetDefaultSharedPreferences(Android.App.Application.Context);
            var contextWrapper = new ContextWrapper(Android.App.Application.Context);
            int currentVersionCode = contextWrapper.PackageManager.GetPackageInfo(contextWrapper.PackageName, 0).VersionCode;
            if (prefs.GetInt("LASTVERSION", 0) < currentVersionCode)
            {
                prefs.Edit().PutInt("LASTVERSION", currentVersionCode).Apply();
                CopyDatabase();
            }

            if (!File.Exists(fullDBPath))
                CopyDatabase();

            return fullDBPath;
        }

        /// <summary>
        /// Copies the database from the application's assets to the device
        /// </summary>
        private void CopyDatabase()
        {
            using (var br = new BinaryReader(Android.App.Application.Context.Assets.Open(DBName)))
            {
                using (var bw = new BinaryWriter(new FileStream(fullDBPath, FileMode.Create)))
                {
                    byte[] buffer = new byte[2048];
                    int length = 0;
                    while ((length = br.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        bw.Write(buffer, 0, length);
                    }
                }
            }
        }
    }
}