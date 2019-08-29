using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KavLachayim.Helpers
{
    /// <summary>
    /// The base class for observable objects
    /// </summary>
    public class ObservableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property to notify listeners.
        /// This value is optional and can be provided automatically when invoked from compilers.
        /// </param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Checks if a property value already matches a desired value.
        /// Sets the property and notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="property">Reference to a property with both a getter and a setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.
        /// This value is optional and can be provided automatically when invoked from compilers that support
        /// CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matches the desired value.</returns>
        protected bool SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(property, value)) return false;

            property = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }
    }
}
