using Xamarin.Forms;
using System.Reflection;
using KavLachayim.Helpers.Extensions;
using KavLachayim.Helpers.MVVM;

namespace KavLachayim.Behaviors
{
    /// <summary>
    /// A behavior used to set the binding context of Views to ViewModels automatically. This class works in cooperation with the ViewModelManager.
    /// </summary>
    public class ViewModelBinderBehavior : BehaviorBase<Element>
    {
        /// <summary>
        /// A bindable property containing the way to set the binding context of a View to a ViewModel
        /// </summary>
        public static readonly BindableProperty BinderBindingModeProperty =
            BindableProperty.Create("BinderBindingMode", typeof(BinderBindingMode), typeof(ViewModelBinderBehavior), BinderBindingMode.None, propertyChanged: OnBinderBindingModePropertyChanged);

        /// <summary>
        /// A bindable property containing the name of the view model to which the view should bind
        /// </summary>
        public static readonly BindableProperty ViewModelNameProperty =
            BindableProperty.Create("ViewModelName", typeof(string), typeof(ViewModelBinderBehavior), null);

        /// <summary>
        /// Getter and setter for the BinderBindingMode bindable property
        /// </summary>
        public BinderBindingMode BinderBindingMode
        {
            set => SetValue(BinderBindingModeProperty, value);
            get => (BinderBindingMode)GetValue(BinderBindingModeProperty);
        }

        /// <summary>
        /// Getter and setter for the ViewModelName bindable property
        /// </summary>
        public string ViewModelName
        {
            set => SetValue(ViewModelNameProperty, value);
            get => (string)GetValue(ViewModelNameProperty);
        }

        /// <summary>
        /// Called when the behavior is attached to a VisualElement
        /// </summary>
        /// <param name="element">The VisualElement to which the behavior was attached</param>
        protected override void OnAttachedTo(Element element)
        {
            base.OnAttachedTo(element);
            BindToViewModel(element);
        }

        /// <summary>
        /// Called when the behavior is detachign from a VisualElement
        /// </summary>
        /// <param name="element">The VisualElement from which the behavior is detaching</param>
        protected override void OnDetachingFrom(Element element)
        {
            base.OnDetachingFrom(element);
            element.BindingContext = null;
        }

        /// <summary>
        /// Binds a View to a ViewModel
        /// </summary>
        /// <param name="element">Used as the View to bind to the ViewModel</param>
        void BindToViewModel(Element element)
        {
            string viewModelName = "MainViewModel";
            switch (BinderBindingMode)
            {
                case BinderBindingMode.ByViewName:
                    viewModelName = element.GetType().Name + "ViewModel";
                    break;
                case BinderBindingMode.ByDefinedName:
                    if (ViewModelName != null && ViewModelName != "")
                        viewModelName = ViewModelName;
                    break;
                case BinderBindingMode.None:
                    element.BindingContext = null;
                    return;
            }

            Assembly runningAssembly = typeof(ViewModelBinderBehavior).GetTypeInfo().Assembly;
            var viewModelType = runningAssembly.GetTypeWithoutNamespace(viewModelName);

            if (App.ViewModelManager.Get(viewModelType) == null)
                App.ViewModelManager.Add(viewModelType);
            element.BindingContext = App.ViewModelManager.Get(viewModelType);
        }

        /// <summary>
        /// Called when the BinderBindingMode bindable-property value changes
        /// </summary>
        /// <param name="bindable">The instance of the behavior for which the BinderBindingMode property changed</param>
        /// <param name="oldValue">Old value of the BinderBindingMode property</param>
        /// <param name="newValue">New value of the BinderBindingMode property</param>
        private static void OnBinderBindingModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (ViewModelBinderBehavior)bindable;
            if (behavior.AssociatedObject == null) return;

            var mode = (BinderBindingMode)newValue;
            behavior.BindToViewModel(behavior.AssociatedObject);
        }
    }
}