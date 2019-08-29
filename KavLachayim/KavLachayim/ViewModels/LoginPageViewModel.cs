using KavLachayim.Helpers;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using Xamarin.Forms;
using KavLachayim.Views;
using KavLachayim.Helpers.MVVM;

namespace KavLachayim.ViewModels
{
    /// <summary>
    /// A ViewModel for the login page
    /// </summary>
    public class LoginPageViewModel : ViewModelBase
    {
        //Variables used for binding purposes to get the username and password the user entered

        private string username;
        public string Username
        {
            set => SetProperty(ref username, value);
            get => username;
        }

        private string password;
        public string Password
        {
            set => SetProperty(ref password, value);
            get => password;
        }

        /// <summary>
        /// A variable used for binding purposes to set the login status
        /// </summary>
        private string loginStatus;
        public string LoginStatus
        {
            set => SetProperty(ref loginStatus, value);
            get => loginStatus;
        }

        private Command loginCommand;
        /// <summary>
        /// A command that performs the login task
        /// </summary>
        public Command LoginCommand
        {
            get
            {
                if (loginCommand == null)
                    loginCommand = new Command(LoginCommandExecute, p => !IsBusy);

                return loginCommand;
            }
        }

        public LoginPageViewModel()
        {
            MessagingCenter.Subscribe<LoginPage>(this, "Appeared", (p) => LoginStatus = null);
        }

        private async void LoginCommandExecute(object parameter)
        {
            IsBusy = true;
            LoginCommand.ChangeCanExecute();
            LoginStatus = "מתחבר...";
            string responseContent = await LoginAsync();

            switch (responseContent)
            {
                case string r when (r == "incorrect" || r == "notfound"):
                    LoginStatus = "הפרטים שהוזנו שגויים או שהמשתמש לא נמצא";
                    break;

                case string r when (r == "exception" || r == "error"):
                    LoginStatus = "אירעה שגיאה בעת ההתחברות";
                    break;

                default:
                    LoginStatus = "ההתחברות הצליחה";
                    Application.Current.Properties["UserToken"] = responseContent;
                    Application.Current.Properties["Username"] = Username;
                    break;
            }
            IsBusy = false;
            LoginCommand.ChangeCanExecute();
        }

        private async Task<string> LoginAsync()
        {
            try
            {
                using (var client = new HttpClient() { Timeout = TimeSpan.FromSeconds(5) })
                {
                    var response = await client.GetAsync($"{Constants.APIUrl}/accountlogin?username={Username}&password={Password}");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        responseContent = responseContent.Remove(0, 1);
                        responseContent = responseContent.Remove(responseContent.Length - 1, 1);
                        return responseContent;
                    }

                    return "error";
                }
            }

            catch (Exception)
            {
                return "exception";
            }
        }
    }
}
