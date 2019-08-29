using System.Collections.Generic;
using KavLachayim.Helpers.MVVM.Interfaces;
using System.Reflection;
using System;
using System.Linq;

namespace KavLachayim.Helpers.MVVM
{
    /// <summary>
    /// A class to manage all ViewModels of the application
    /// </summary>
    public class ViewModelManager
    {
        private Dictionary<string, IViewModel> ViewModels = new Dictionary<string, IViewModel>();
        private static ViewModelManager instance;
        private static object thelock = new object();

        private ViewModelManager() { }

        public static ViewModelManager Manager
        {
            get
            {
                if (instance == null)
                {
                    lock (thelock)
                    {
                        if (instance == null)
                            instance = new ViewModelManager();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Adds a ViewModel to the ViewModelManager managed ViewModels dictionary
        /// </summary>
        /// <param name="ViewModel">The ViewModel type to add</param>
        public void Add(Type ViewModel)
        {
            if (ViewModel != null)
            {
                if (typeof(IViewModel).GetTypeInfo().IsAssignableFrom(ViewModel.GetTypeInfo()))
                {
                    if (Get(ViewModel) == null)
                        ViewModels.Add(ViewModel.Name, (IViewModel)Activator.CreateInstance(ViewModel));
                }

                else throw new InvalidOperationException("A valid ViewModel class must derive from IViewModel");
            }

            else throw new InvalidOperationException("ViewModel type not found in assembly");
        }

        /// <summary>
        /// Gets a ViewModel from the ViewModelManager managed ViewModels dictionary
        /// </summary>
        /// <param name="ViewModel">Specifies the type of the ViewModel to be returned</param>
        public IViewModel Get(Type ViewModel)
        {
            if (ViewModels.Keys.Contains(ViewModel.Name))
                return ViewModels[ViewModel.Name];
            else return null;
        }
    }
}
