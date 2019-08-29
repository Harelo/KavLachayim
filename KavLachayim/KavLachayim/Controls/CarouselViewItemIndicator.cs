using Xamarin.Forms;
using System.Collections;
using System.ComponentModel;
using CarouselView.FormsPlugin.Abstractions;

namespace KavLachayim.Controls
{
    /// <summary>
    /// A control to be attached to a CarouselView to present its' position
    /// </summary>
    public class CarouselViewItemIndicator : StackLayout
    {
        /// <summary>
        /// An internal class to be used for the control in order to set the indicator
        /// </summary>
        private class DotButton : BoxView
        {
            public int Index;
            public event ClickHandler Clicked;
            public delegate void ClickHandler(DotButton sender);

            public DotButton()
            {
                var clickCheck = new TapGestureRecognizer() { Command = new Command(() => Clicked?.Invoke(this)) };
                GestureRecognizers.Add(clickCheck);
            }
        }

        /// <summary>
        /// A bindable property which determines which color the indicator should be
        /// </summary>
        public static readonly BindableProperty IndicatorColorProperty = BindableProperty.Create
            ("IndicatorColor", typeof(Color), typeof(CarouselViewItemIndicator), Color.Default, propertyChanged: IndicatorColorPropertyChanged);

        /// <summary>
        /// An internal property to be used by the control in order to set the indicator
        /// </summary>
        private DotButton[] Dots { get; set; }

        private CarouselViewControl carousel;
        /// <summary>
        /// The CarouselView the control should be connected to
        /// </summary>
        public CarouselViewControl Carousel
        {
            set
            {
                carousel = value;
                Carousel.PropertyChanged += OnCarouselPropertyChanged;

                if (Carousel.ItemsSource != null)
                {
                    var carouselItemCount = ((ICollection)Carousel.ItemsSource).Count;
                    InitDots(carouselItemCount);
                }
            }
            get => carousel;
        }

        public Color IndicatorColor
        {
            set => SetValue(IndicatorColorProperty, value);
            get => (Color)GetValue(IndicatorColorProperty);
        }

        public CarouselViewItemIndicator()
        {
            Orientation = StackOrientation.Horizontal;
            VerticalOptions = LayoutOptions.Center;
            HorizontalOptions = LayoutOptions.Center;
            BackgroundColor = Color.Transparent;

            if (Carousel != null)
            {
                var carouselItemCount = ((ICollection)Carousel.ItemsSource).Count;
                InitDots(carouselItemCount);
            }
        }

        /// <summary>
        /// Used to initialize the "dots" for the indicator
        /// </summary>
        /// <param name="amount"></param>
        private void InitDots(int amount)
        {
            Dots = new DotButton[amount];

            for (int i = 0; i < amount; i++)
            {
                Dots[i] = new DotButton
                {
                    HeightRequest = 10,
                    WidthRequest = 10,
                    BackgroundColor = IndicatorColor,
                    Opacity = 0.5
                };

                Dots[i].Index = i;
                Dots[i].Clicked += (s) => Carousel.Position = s.Index;
                Children.Add(Dots[i]);
            }

            Dots[Carousel.Position].Opacity = 1;
            Dots[Carousel.Position].ScaleTo(1.2, 75);
        }

        /// <summary>
        /// Called when the "Position" property of the CarouselView the control is attached to has changed
        /// </summary>
        /// <param name="newPosition">The new position property value</param>
        private void OnPositionChanged(int newPosition)
        {
            if (Carousel.ItemsSource == null) return;

            for (int i = 0; i < Dots.Length; i++)
            {
                if (newPosition == i)
                {
                    Dots[i].Opacity = 1;
                    Dots[i].ScaleTo(1.2, 75);
                }
                else
                {
                    Dots[i].Opacity = 0.5;
                    Dots[i].ScaleTo(1, 75);
                }
            }
        }

        private static void IndicatorColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var cvii = (CarouselViewItemIndicator)bindable; //CVII = CarouselViewItemIndicator
            if (cvii.Dots != null)
            {
                for (int i = 0; i < cvii.Dots.Length; i++)
                    cvii.Dots[i].BackgroundColor = (Color)newValue;
            }
        }

        /// <summary>
        /// Called when a property of the CarouselView the control is attached to is changed
        /// </summary>
        /// <param name="sender">Indicates the object that caused the invocation of the method</param>
        /// <param name="e">Contains information about the property that has changed</param>
        private void OnCarouselPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ItemsSource":
                    var carouselItemCount = ((ICollection)Carousel.ItemsSource).Count;
                    InitDots(carouselItemCount);
                    break;

                case "Position":
                    OnPositionChanged(Carousel.Position);
                    break;
            }
        }
    }
}