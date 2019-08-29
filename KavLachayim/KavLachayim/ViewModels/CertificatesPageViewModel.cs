using KavLachayim.Helpers.MVVM;
using System.Collections.ObjectModel;
using static KavLachayim.Data.KavLachayimDB;
using Xamarin.Forms;
using System;

namespace KavLachayim.ViewModels
{
    public class CertificatesPageViewModel : ViewModelBase
    {
        private ObservableCollection<FormsTR> formsCollection;
        /// <summary>
        /// Holds a collection of records from the Forms table
        /// </summary>
        public ObservableCollection<FormsTR> FormsCollection
        {
            set => SetProperty(ref formsCollection, value);
            get => formsCollection;
        }

        private ObservableCollection<ApprovalFormsTR> approvalFormsCollection;
        /// <summary>
        /// Holds a collection of records from the ApprovalForms table
        /// </summary>
        public ObservableCollection<ApprovalFormsTR> ApprovalFormsCollection
        {
            set => SetProperty(ref approvalFormsCollection, value);
            get => approvalFormsCollection;
        }

        private FormsTR selectedForm;
        /// <summary>
        /// Stores the last selected item from FormsList
        /// </summary>
        public FormsTR SelectedForm
        {
            set
            {
                if (value == null) return;

                SetProperty(ref selectedForm, value);

                Device.OpenUri(new Uri(SelectedForm.UrlAddress));
            }

            get => selectedForm;
        }

        private ApprovalFormsTR selectedApprovalForm;
        /// <summary>
        /// Stores the last selected item from ApprovalFormsList
        /// </summary>
        public ApprovalFormsTR SelectedApprovalForm
        {
            set
            {
                if (value == null) return;

                SetProperty(ref selectedApprovalForm, value);

                Device.OpenUri(new Uri(SelectedApprovalForm.UrlAddress));
            }

            get => selectedApprovalForm;
        }

        public CertificatesPageViewModel()
        {
            ListView a = new ListView();
            async void GetCertificates()
            {
                FormsCollection = await App.Database.GetTableAsync<FormsTR>();
                ApprovalFormsCollection = await App.Database.GetTableAsync<ApprovalFormsTR>();
            }

            GetCertificates();
        }
    }
}
