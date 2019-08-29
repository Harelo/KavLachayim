using System;
using System.Threading;

namespace KavLachayim.Models
{
    /// <summary>
    /// A class that represents a MasterDetailPage's menu item's data
    /// </summary>
    public class MDPageMenuItemModel
    {
        public MDPageMenuItemModel()
        {
            this.TargetType = typeof(MainPage);
        }

        public string Description { get; set; }
        public string Title { get; set; }
        public int ID { get; set; }
        public Type TargetType { get; set; }
    }
}