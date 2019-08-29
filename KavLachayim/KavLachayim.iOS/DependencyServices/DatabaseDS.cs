using Xamarin.Forms;
using KavLachayim.DependencyInterfaces;
using KavLachayim.iOS.DependencyServices;
using System;
using System.IO;
using Foundation;

[assembly: Dependency(typeof(DatabaseDS))]
namespace KavLachayim.iOS.DependencyServices
{
    /// <summary>
    /// A class used with a dependency service to perform operations on the database
    /// </summary>
    public class DatabaseDS : IDatabaseDS
    {
        private string fullDBPath, DBName;

        public string GetDatabasePath(string dbName)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");
            if (!Directory.Exists(libFolder))
                Directory.CreateDirectory(libFolder);
            DBName = dbName;
            fullDBPath = Path.Combine(libFolder, DBName);

            //  Checks the current version code, if it's higher then the previous stored one, save the new one
            //  and update the database
            var prefs = NSUserDefaults.StandardUserDefaults;
            var currentVersionNumber = nint.Parse(NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString());

            if (prefs.ValueForKey(new NSString("LASTVERSION")) == null) prefs.SetInt(0, "LASTVERSION");

            if (prefs.IntForKey("LASTVERSION") < currentVersionNumber)
            {
                prefs.SetInt(currentVersionNumber, "LASTVERSION");
                CopyDatabase();
            }

            return fullDBPath;
        }

        private void CopyDatabase()
        {
            string extension = Path.GetExtension(fullDBPath);
            var dbResourcePath = NSBundle.MainBundle.PathForResource(DBName.Replace(extension, null), extension.Remove(0, 1));
            File.Copy(dbResourcePath, fullDBPath);
        }
    }
}