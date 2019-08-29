using KavLachayim.Helpers;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace KavLachayim.Controls
{
    /// <summary>
    /// A custom StackLayout that allows an items source and template to be defined
    /// </summary>
    public class RepeatingStackLayout : StackLayout
    {
        public event EventHandler<ItemSelectedEventArgs> ItemSelected;

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            "ItemsSource",
            typeof(IEnumerable),
            typeof(RepeatingStackLayout),
            propertyChanged: ItemsSourcePropertyChanged);


        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
            "ItemTemplate",
            typeof(DataTemplate),
            typeof(RepeatingStackLayout));

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            "Command",
            typeof(ICommand),
            typeof(RepeatingGridLayout));

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            "CommandParameter",
            typeof(object),
            typeof(RepeatingGridLayout));

        public static readonly BindableProperty SelectedItemIndexProperty = BindableProperty.Create(
            "SelectedItemIndex",
            typeof(int),
            typeof(RepeatingGridLayout),
            0);

        public IEnumerable ItemsSource
        {
            set => SetValue(ItemsSourceProperty, value);
            get => (IEnumerable)GetValue(ItemsSourceProperty);
        }

        public DataTemplate ItemTemplate
        {
            set => SetValue(ItemTemplateProperty, value);
            get => (DataTemplate)GetValue(ItemTemplateProperty);
        }

        public ICommand Command
        {
            set => SetValue(CommandProperty, value);
            get => (ICommand)GetValue(CommandProperty);
        }

        public object CommandParameter
        {
            set => SetValue(CommandParameterProperty, value);
            get => GetValue(CommandParameterProperty);
        }

        public int SelectedItemIndex
        {
            private set => SetValue(SelectedItemIndexProperty, value);
            get => (int)GetValue(SelectedItemIndexProperty);
        }

        ObservableCollection<object> observableSource;
        protected ObservableCollection<object> ObservableSource
        {
            set
            {
                if (observableSource != null)
                    observableSource.CollectionChanged -= ObservableSourceCollectionChanged;

                observableSource = value;

                if (observableSource != null)
                    observableSource.CollectionChanged += ObservableSourceCollectionChanged;
            }

            get => observableSource;
        }

        private static void ItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var layout = (RepeatingStackLayout)bindable;
            layout.SetItemsFromSource();
        }

        protected virtual View GetItemView(object item)
        {
            var content = ItemTemplate.CreateContent();

            var view = content as View;
            if (view == null)
                return null;

            view.BindingContext = item;
            return view;
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            var view = (View)child;
            var gesture = new TapGestureRecognizer();
            gesture.Tapped += (s, e) =>
            {
                SelectedItemIndex = Children.IndexOf(view);
                ItemSelected?.Invoke(this, new ItemSelectedEventArgs(SelectedItemIndex, view));
                if (Command?.CanExecute(CommandParameter) == true)
                    Command.Execute(CommandParameter);
            };
            view.GestureRecognizers.Add(gesture);
        }

        private void SetItemsFromSource()
        {
            Children.Clear();

            if (ItemsSource == null)
            {
                ObservableSource = null;
                return;
            }

            foreach (var item in ItemsSource)
                Children.Add(GetItemView(item));

            var t = ItemsSource.GetType();
            var isObservableCollection = t.IsConstructedGenericType && t.GetGenericTypeDefinition() == typeof(ObservableCollection<>);
            if (isObservableCollection)
                ObservableSource = new ObservableCollection<object>(ItemsSource.Cast<object>());
        }

        private void ObservableSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    int index = e.NewStartingIndex;
                    foreach (var item in e.NewItems)
                        Children.Insert(index++, GetItemView(item));
                    break;

                case NotifyCollectionChangedAction.Move:
                    var theItem = ObservableSource[e.OldStartingIndex];
                    Children.RemoveAt(e.OldStartingIndex);
                    Children.Insert(e.NewStartingIndex, GetItemView(theItem));
                    break;

                case NotifyCollectionChangedAction.Remove:
                    Children.RemoveAt(e.OldStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    Children.RemoveAt(e.OldStartingIndex);
                    Children.Insert(e.NewStartingIndex, GetItemView(ObservableSource[e.NewStartingIndex]));
                    break;

                case NotifyCollectionChangedAction.Reset:
                    Children.Clear();
                    foreach (var item in ItemsSource)
                        Children.Add(GetItemView(item));
                    break;
            }
        }
    }
}
