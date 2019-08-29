using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace KavLachayim.Behaviors
{
    public class EventToCommandBehavior : BehaviorBase<View>
    {
        Delegate eventHandler;

        //Bindable properties used for the behavior
        public static readonly BindableProperty EventNameProperty =
            BindableProperty.Create("EventName", typeof(string), typeof(EventToCommandBehavior), propertyChanged: OnEventNameChanged);

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(EventToCommandBehavior));

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(object), typeof(EventToCommandBehavior));

        public static readonly BindableProperty ConverterProperty =
            BindableProperty.Create("Converter", typeof(IValueConverter), typeof(EventToCommandBehavior));

        //Properties used to get and set the values of the bindable properties of this behavior

        public string EventName
        {
            set => SetValue(EventNameProperty, value);
            get => (string)GetValue(EventNameProperty);
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

        public IValueConverter Converter
        {
            set => SetValue(ConverterProperty, value);
            get => (IValueConverter)GetValue(ConverterProperty);
        }

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(EventName);
        }

        protected override void OnDetachingFrom(View bindable)
        {
            DeregisterEvent(EventName);
            base.OnDetachingFrom(bindable);
        }

        void RegisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return;

            EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
            if (eventInfo == null) throw new ArgumentException($"EventToCommandBehavior: Can't register the {name} event.");

            MethodInfo methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
            eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(AssociatedObject, eventHandler);
        }

        void DeregisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return;

            if (eventHandler == null) return;

            EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
            if (eventInfo == null) throw new ArgumentException($"EventToCommandBehavior: Can't de-register the {name} event.");

            eventInfo.RemoveEventHandler(AssociatedObject, eventHandler);
            eventHandler = null;
        }

        void OnEvent(object sender, object eventArgs)
        {
            if (Command == null) return;

            object resolvedParameter;
            if (CommandParameter != null) resolvedParameter = CommandParameter;
            else if (Converter != null) resolvedParameter = Converter.Convert(eventArgs, typeof(object), null, null);
            else resolvedParameter = eventArgs;

            if (Command.CanExecute(resolvedParameter)) Command.Execute(resolvedParameter);
        }

        static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (EventToCommandBehavior)bindable;
            if (behavior.AssociatedObject == null) return;

            string oldEventName = (string)oldValue;
            string newEventName = (string)newValue;

            behavior.DeregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName);
        }
    }
}