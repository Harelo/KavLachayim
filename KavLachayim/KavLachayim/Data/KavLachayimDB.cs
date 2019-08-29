using SQLite;
using System.Threading.Tasks;
using Xamarin.Forms;
using KavLachayim.DependencyInterfaces;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Net;
using KavLachayim.Helpers;
using System;
using System.Net.Http;
using System.Linq;
using System.ComponentModel;

namespace KavLachayim.Data
{
    /// <summary>
    /// Represents the application's database
    /// </summary>
    public class KavLachayimDB
    {
        private static SQLiteAsyncConnection dbConnection;
        private static KavLachayimDB instance;
        private static object thelock = new object();

        public static KavLachayimDB Database
        {
            get
            {
                if (instance == null)
                {
                    lock (thelock)
                    {
                        if (instance == null)
                            instance = new KavLachayimDB();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// A private constructor for the KavLachayimDB class
        /// </summary>
        private KavLachayimDB()
        {
            string dbPath = DependencyService.Get<IDatabaseDS>().GetDatabasePath(Constants.DatabaseName);
            dbConnection = new SQLiteAsyncConnection(dbPath);
        }

        /// <summary>
        /// Checks for updates in the database by contacting the server
        /// </summary>
        /// <returns>True if found update, false otherwise</returns>
        public async Task<bool> CheckDBUpdatesOnlineAsync()
        {
            if (!App.IsConnected) return false;

            try
            {
                using (var httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(5) })
                {
                    int appVersion = DependencyService.Get<IAppInfoDS>().Version;
                    var response = await httpClient.GetAsync($"{Constants.APIUrl}/database/{appVersion}/version");

                    if (!response.IsSuccessStatusCode) return false;

                    int deviceDBVer = (await dbConnection.Table<DatabaseInformationTable>().ToListAsync()).First().Version;
                    int onlineDBVer = int.Parse(await response.Content.ReadAsStringAsync());

                    //Check if the DB version on the device is lower than the one online
                    if (deviceDBVer < onlineDBVer) return true;
                }
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An exception occured while trying to contact the server: {ex.Message}");
                if (ex.InnerException != null)
                    System.Diagnostics.Debug.WriteLine($"With an InnerException: {ex.InnerException.Message}");
            }

            return false;
        }

        /// <summary>
        /// Downloads an updated database from the server and replaces the one on the device with it
        /// </summary>
        /// <returns></returns>
        public async Task UpdateDBFromServer(DownloadProgressChangedEventHandler downloadProgressChangedHandler = null)
        {
            try
            {
                if (App.IsConnected)
                {
                    using (var httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(5) })
                    {
                        int appVersion = DependencyService.Get<IAppInfoDS>().Version;
                        var response = httpClient.GetAsync($"{Constants.APIUrl}/database/{appVersion}/version").Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var webClient = new WebClient();
                            string fullDBPath = DependencyService.Get<IDatabaseDS>().GetDatabasePath(Constants.DatabaseName);

                            if (downloadProgressChangedHandler != null)
                                webClient.DownloadProgressChanged += downloadProgressChangedHandler;
                            await webClient.DownloadFileTaskAsync(new Uri($"{Constants.APIUrl}/database/{appVersion}/download"), fullDBPath);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An exception occured while trying to contact the server: {ex.Message}");
                if (ex.InnerException != null)
                    System.Diagnostics.Debug.WriteLine($"With an InnerException: {ex.InnerException.Message}");
            }
        }

        /// <summary>
        /// Returns all records in a table as an ObservableCollection
        /// </summary>
        /// <typeparam name="T">The type that represents a single table record for which you want to obtain the table</typeparam>
        /// <returns>An ObservableCollection of all records in the table</returns>
        public async Task<ObservableCollection<T>> GetTableAsync<T>() where T : new()
        {
            List<T> records = await dbConnection.Table<T>().ToListAsync();
            return new ObservableCollection<T>(records);
        }

        /// <summary>
        /// Returns all the names of the campaigns the user can donate to
        /// </summary>
        /// <returns></returns>
        public async Task<ObservableCollection<string>> GetDonateToOptionsAsync()
        {
            var campaignsTitles = (await dbConnection.Table<CampaignsTR>().ToListAsync()).Where(r => !r.Finished).Select(r => r.Title);
            var options = new ObservableCollection<string>(campaignsTitles);
            options.Insert(0, "קו לחיים");
            return options;
        }

        /// <summary>
        /// Executes an SQL statement that is expected to return a value
        /// </summary>
        /// <typeparam name="T">The expected return value's type</typeparam>
        /// <param name="SQLstr">The SQL statement to execute</param>
        /// <param name="args">Optional arguments</param>
        /// <returns>Returns the first column of the first row in the result set by the query</returns>
        public async Task<T> ExecuteScalarAsync<T>(string SQLstr, params object[] args) => await dbConnection.ExecuteScalarAsync<T>(SQLstr);

        //Below are classes that represents records in tables in the database.
        //Each class represents a single table. Each instance of the class represents a single record in that table

        [Table("InfoPCT")]
        public class InfoPageContentTR
        {
            [Unique, PrimaryKey, AutoIncrement, Column("ID")]
            public int ID { set; get; }

            [Unique, Column("Title")]
            public string Title { set; get; }

            [Column("Content")]
            public string Content { set; get; }

        }

        [Table("DonationOptionsPCT")]
        public class DonationOptionsTR
        {
            [Unique, PrimaryKey, AutoIncrement, Column("ID")]
            public int ID { set; get; }

            [Column("OptionName")]
            public string OptionName { set; get; }

            [Column("Content")]
            public string Content { set; get; }
        }

        [Table("ContactUsPCT")]
        public class ContactUsTR
        {
            [AutoIncrement, Unique, PrimaryKey, Column("ID")]
            public int ID { set; get; }

            [Unique, Column("BranchName")]
            public string BranchName { set; get; }

            [Column("StreetName")]
            public string StreetName { set; get; }

            [Column("PhoneNumber")]
            public string PhoneNumber { set; get; }

            [Column("Email")]
            public string Email { set; get; }

            [Column("OtherInfo")]
            public string OtherInfo { set; get; }
        }

        [Table("AboutPCT")]
        public class AboutTR
        {
            [AutoIncrement, PrimaryKey, Unique, Column("ID")]
            public int ID { set; get; }

            [Unique, Column("Title")]
            public string Title { set; get; }

            [Column("Content")]
            public string Content { set; get; }
        }

        [Table("CampaignsPCT")]
        public class CampaignsTR
        {
            [AutoIncrement, PrimaryKey, Unique, Column("ID")]
            public int ID { set; get; }

            [Unique, Column("Title")]
            public string Title { set; get; }

            [Column("DisplayImage")]
            public byte[] DisplayImage { set; get; }

            [Column("Content")]
            public string Content { set; get; }

            [Column("ContentImage")]
            public byte[] ContentImage { set; get; }

            [Column("Finished")]
            public bool Finished { set; get; }

            [Column("FinishedReason")]
            public string FinishedReason { set; get; }
        }

        [Table("ArticlesAboutUsPCT")]
        public class ArticlesAboutUsTR : ObservableBase
        {
            [AutoIncrement, PrimaryKey, Unique, Column("ID")]
            public int ID { set; get; }

            [Column("Title")]
            public string Title { set; get; }

            [Column("UrlAddress")]
            public string UrlAddress { set; get; }

            [Column("Description")]
            public string Description { set; get; }

            [Column("Image")]
            public byte[] Image { set; get; }
        }

        [Table("EmployeesPCT")]
        public class EmployeesTR
        {
            [AutoIncrement, PrimaryKey, Unique, Column("ID")]
            public int ID { set; get; }

            [Column("Name")]
            public string Name { set; get; }

            [Column("JobDescription")]
            public string JobDescription { set; get; }

            [Column("Image")]
            public byte[] Image { set; get; }
        }

        [Table("ActivitiesPCT")]
        public class ActivitiesTR
        {
            [AutoIncrement, PrimaryKey, Unique, Column("ID")]
            public int ID { set; get; }

            [Column("Title")]
            public string Title { set; get; }

            [Column("Content")]
            public string Content { set; get; }
        }

        [Table("ThanksPCT")]
        public class ThanksTR
        {
            [Column("ImageSource")]
            public byte[] ImageSource { set; get; }
        }

        [Table("Forms")]
        public class FormsTR
        {
            [AutoIncrement, PrimaryKey, Unique, Column("ID")]
            public int ID { set; get; }

            [Column("Name")]
            public string Name { set; get; }

            [Column("UrlAddress")]
            public string UrlAddress { set; get; }
        }

        [Table("ApprovalForms")]
        public class ApprovalFormsTR
        {
            [AutoIncrement, PrimaryKey, Unique, Column("ID")]
            public int ID { set; get; }

            [Column("Name")]
            public string Name { set; get; }

            [Column("UrlAddress")]
            public string UrlAddress { set; get; }
        }

        [Table("DatabaseInformation")]
        public class DatabaseInformationTable
        {
            [Column("Version")]
            public int Version { set; get; }
        }
    }
}