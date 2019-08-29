using KavLachayim.ViewModels;
using System.Windows.Input;

namespace KavLachayim.Models
{
    /// <summary>
    /// A class to represent data to be presented in a specific CarouselView
    /// </summary>
    public class MainPageCarouselModel
    {
        public string ImageSource { get; set; }
        public object OnClickParameter { get; set; }
        public ICommand InfoImgTapCommand
        {
            get
            {
                return ((MainPageViewModel)App.ViewModelManager.Get(typeof(MainPageViewModel))).InfoImgTapCommand;
            }
        }
    }
}
