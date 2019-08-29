using System.Collections.ObjectModel;

namespace KavLachayim.Models
{
    public class GroupedCertificatesModel<ITable> : ObservableCollection<ITable>
    {
        public string LongName { set; get; }
        public string ShortName { set; get; }

        public GroupedCertificatesModel(ObservableCollection<ITable> collection) : base(collection) { }
    }
}
