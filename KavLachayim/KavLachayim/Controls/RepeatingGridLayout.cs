using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using System.Linq;
using KavLachayim.Helpers;
using System.Windows.Input;
using System.Reflection;
using System.Threading.Tasks;

namespace KavLachayim.Controls
{
    /// <summary>
    /// Holds data about the layout
    /// </summary>
    public struct LayoutData
    {
        public int VisibleChildCount { get; private set; }
        public Size CellSize { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public LayoutData(int _visibleChildCount, Size _cellSize, int _rows, int _columns) : this()
        {
            VisibleChildCount = _visibleChildCount;
            CellSize = _cellSize;
            Rows = _rows;
            Columns = _columns;
        }
    }

    /// <summary>
    /// A custom layout displaying data in a grid and allowing children to be clickable. An ItemsSource and ItemTemplate can be specified.
    /// </summary>
    public class RepeatingGridLayout : Layout<View>
    {
        public event EventHandler<ItemSelectedEventArgs> ItemSelected;

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            "ItemsSource",
            typeof(IEnumerable),
            typeof(RepeatingGridLayout),
            propertyChanged: ItemsSourcePropertyChanged);

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
            "ItemTemplate",
            typeof(DataTemplate),
            typeof(RepeatingGridLayout));

        public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create(
            "ColumnSpacing",
            typeof(double),
            typeof(RepeatingGridLayout),
            5.0,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                ((RepeatingGridLayout)bindable).InvalidateLayout();
            });

        public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create(
            "RowSpacing",
            typeof(double),
            typeof(RepeatingGridLayout),
            5.0,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                ((RepeatingGridLayout)bindable).InvalidateLayout();
            });

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

        public double ColumnSpacing
        {
            set => SetValue(ColumnSpacingProperty, value);
            get => (double)GetValue(ColumnSpacingProperty);
        }

        public double RowSpacing
        {
            set => SetValue(RowSpacingProperty, value);
            get => (double)GetValue(RowSpacingProperty);
        }

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

        Dictionary<Size, LayoutData> layoutDataCache = new Dictionary<Size, LayoutData>();

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

        public LayoutData GetLayoutData(double width, double height)
        {
            Size size = new Size(width, height);
            if (layoutDataCache.ContainsKey(size))
                return layoutDataCache[size];

            int visibleChildCount = 0;
            Size maxChildSize = new Size();
            int rows = 0;
            int columns = 0;
            LayoutData layoutData = new LayoutData();

            foreach (View child in Children)
            {
                if (!child.IsVisible)
                    continue;

                visibleChildCount++;

                SizeRequest childSizeRequest = child.Measure(double.PositiveInfinity, double.PositiveInfinity);
                maxChildSize.Width = Math.Max(maxChildSize.Width, childSizeRequest.Request.Width);
                maxChildSize.Height = Math.Max(maxChildSize.Height, childSizeRequest.Request.Height);
            }

            if (visibleChildCount != 0)
            {
                if (double.IsPositiveInfinity(width))
                {
                    columns = visibleChildCount;
                    rows = 1;
                }

                else
                {
                    columns = (int)((width + ColumnSpacing) / (maxChildSize.Width + ColumnSpacing));
                    columns = Math.Max(1, columns);
                    rows = (visibleChildCount + columns - 1) / columns;
                }

                Size cellSize = new Size();
                if (double.IsPositiveInfinity(width))
                    cellSize.Width = maxChildSize.Width;
                else
                    cellSize.Width = (width - ColumnSpacing * (columns - 1)) / columns;

                if (double.IsPositiveInfinity(height))
                    cellSize.Height = maxChildSize.Height;
                else
                    cellSize.Height = (height - RowSpacing * (rows - 1)) / rows;

                layoutData = new LayoutData(visibleChildCount, cellSize, rows, columns);
            }

            layoutDataCache.Add(size, layoutData);
            return layoutData;
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var layoutData = GetLayoutData(widthConstraint, heightConstraint);
            if (layoutData.VisibleChildCount == 0)
                return new SizeRequest();

            Size totalSize = new Size(layoutData.CellSize.Width * layoutData.Columns + ColumnSpacing * (layoutData.Columns - 1),
                layoutData.CellSize.Height * layoutData.Rows + RowSpacing * (layoutData.Rows - 1));

            return new SizeRequest(totalSize);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            var layoutData = GetLayoutData(width, height);

            if (layoutData.VisibleChildCount == 0)
                return;

            double xChild = x;
            double yChild = y;
            int row = 0;
            int column = 0;

            foreach (View child in Children)
            {
                if (!child.IsVisible)
                    continue;

                LayoutChildIntoBoundingRegion(child, new Rectangle(new Point(xChild, yChild), layoutData.CellSize));

                if (++column == layoutData.Columns)
                {
                    column = 0;
                    row++;
                    xChild = x;
                    yChild += RowSpacing + layoutData.CellSize.Height;
                }

                else
                    xChild += ColumnSpacing + layoutData.CellSize.Width;
            }
        }

        protected override void InvalidateLayout()
        {
            base.InvalidateLayout();
            layoutDataCache.Clear();
        }

        protected override void OnChildMeasureInvalidated()
        {
            base.OnChildMeasureInvalidated();
            layoutDataCache.Clear();
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            var view = (View)child;
            var gesture = new TapGestureRecognizer();
            view.GestureRecognizers.Add(gesture);
            gesture.Tapped += (s, e) =>
            {
                SelectedItemIndex = Children.IndexOf(view);
                ItemSelected?.Invoke(this, new ItemSelectedEventArgs(SelectedItemIndex, view));
                if (Command?.CanExecute(CommandParameter) == true)
                    Command.Execute(CommandParameter);
            };
        }

        protected virtual void SetItemsFromSource()
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

        private View GetItemView(object item)
        {
            var content = ItemTemplate.CreateContent();

            var view = content as View;
            if (view == null)
                return null;

            view.BindingContext = item;
            return view;
        }

        private static void ItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var grid = (RepeatingGridLayout)bindable;
            grid.SetItemsFromSource();
        }

        private void ObservableSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    int index = e.NewStartingIndex;
                    foreach (var item in e.NewItems)
                        Children.Insert(index, GetItemView(item));
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