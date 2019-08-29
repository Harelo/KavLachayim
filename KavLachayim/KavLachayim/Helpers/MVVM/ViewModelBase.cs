using KavLachayim.Helpers.MVVM.Interfaces;

namespace KavLachayim.Helpers.MVVM
{
    public class ViewModelBase : ObservableBase, IViewModel
    {
        /// <summary>
        /// Indicates whether the ViewModel is currently busy
        /// </summary>
        public bool IsBusy { get; set; }
    }
}
