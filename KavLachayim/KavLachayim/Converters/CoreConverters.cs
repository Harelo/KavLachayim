using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KavLachayim.Converters
{
    /// <summary>
    /// A converter that converts binary data to an ImageSource object which can then be consumed by an Image control
    /// </summary>
    public class BinaryToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] bmp = value as byte[];
            if (bmp == null)
                return null;

            return ImageSource.FromStream(() => new MemoryStream(bmp));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// A converter that converts a URL into the title of it by getting the HTML data of that URL
    /// </summary>
    public class UrlToTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!App.IsConnected)
                return "אינך מחובר לאינטרנט";
            else
            {
                try
                {
                    WebRequest request = WebRequest.Create(value.ToString());
                    Task<WebResponse> responseTask = Task.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null);
                    WebResponse response = responseTask.Result;
                    Stream stream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(stream);
                    string source = reader.ReadToEnd();
                    string title = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
                    return (title == null || title == "") ? value.ToString() : title;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("An exception occured in Converters/CoreConverters: " + ex.Message);
                }
            }
            return "שגיאה אירעה";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InvertBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;
    }
}
