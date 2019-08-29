using System.Collections.ObjectModel;
using KavLachayim.Models;
using KavLachayim.Views;
using KavLachayim.Helpers.MVVM;

namespace KavLachayim.ViewModels
{
    /// <summary>
    /// A ViewModel for the MDPageMaster page
    /// </summary>
    public class MDPageMasterViewModel : ViewModelBase
    {
        /// <summary>
        /// A collection of items for the menu, each represents a single page
        /// </summary>
        public ObservableCollection<MDPageMenuItemModel> MenuItems { get; set; }

        public MDPageMasterViewModel()
        {
            MenuItems = new ObservableCollection<MDPageMenuItemModel>(new[]
            {
                new MDPageMenuItemModel { ID = 0, Description = "דף הבית", TargetType = typeof(MainPage)},
                new MDPageMenuItemModel { ID = 1, Description = "אודותינו", Title = "אודותינו", TargetType = typeof(AboutPage)},
                new MDPageMenuItemModel { ID = 2, Description = "קמפיינים", Title = "קמפיינים מצילי חיים", TargetType = typeof(CampaignsPage)},
                new MDPageMenuItemModel { ID = 3, Description = "תרומות", Title = "אפשרויות לתרומה", TargetType = typeof(DonateTabbedPage)},
                new MDPageMenuItemModel { ID = 4, Description = "פעילויות", Title = "פעילויות", TargetType = typeof(ActivitiesPage)},
                new MDPageMenuItemModel { ID = 5, Description = "מדברים עלינו", Title = "מדברים עלינו", TargetType = typeof(ArticlesAboutUs)},
                new MDPageMenuItemModel { ID = 6, Description = "תודות ושותפים", Title = "תודות ושותפים", TargetType = typeof(ThanksPage)},
                new MDPageMenuItemModel { ID = 7, Description = "טפסים ואישורי עמותה", Title = "טפסים ואישורי עמותה", TargetType = typeof(CertificatesPage)},
                new MDPageMenuItemModel { ID = 8, Description = "צור קשר", Title = "צור קשר", TargetType = typeof(ContactUsPage)},
                new MDPageMenuItemModel { ID = 9, Description = "תקנון האפליקציה", Title = "תקנון האפליקציה", TargetType = typeof(TermsOfUsePage)}
            });
        }
    }
}
