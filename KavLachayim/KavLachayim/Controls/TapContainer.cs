using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace KavLachayim.Controls
{
    /// <summary>
    ///  A custom ContentView view which allows views to be clickable and an action to happen when they are clicked
    /// </summary>
    public class TapContainer : ContentView
    {
        //Defines an OnClick bindable property of type command
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(TapContainer));
        //Defines an OnClickParameter bindable property of type Object
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(TapContainer));

        public event EventHandler Tapped;

        //Exposes the bindable properties - defines a set and get functions for it
        public ICommand Command
        {
            set { SetValue(CommandProperty, value); }
            get { return (ICommand)GetValue(CommandProperty); }
        }

        public object CommandParameter
        {
            set { SetValue(CommandParameterProperty, value); }
            get { return (object)GetValue(CommandParameterProperty); }
        }

        //The constructor for TapContainer which adds a Gesture to the ContentView
        public TapContainer()
        {
            GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(TapAction), NumberOfTapsRequired = 1 });
        }

        //A method that will be ran when the ContentView is clicked and will call the command defined in the property OnClick and/or the event Clicked
        private void TapAction()
        {
            if (Command != null)
                Command.Execute(CommandParameter);

            Tapped?.Invoke(this, EventArgs.Empty);
        }

        public void ClearClickedEventHandlers()
        {
            Tapped = null;
        }
    }
}
