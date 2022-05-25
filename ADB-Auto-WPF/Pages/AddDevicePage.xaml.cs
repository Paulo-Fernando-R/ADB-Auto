using ADB.Core.Models;
using ADB.Core.Repositories.Interfaces;
using ModernWpf.Controls;
using System.Net;
using System.Windows;

namespace ADB_Auto_WPF.Pages
{
    /// <summary>
    /// Interaction logic for AddDevicePage.xaml
    /// </summary>
    public partial class AddDevicePage : System.Windows.Controls.Page
    {
        private readonly IIPRespository _ipRepository;

        public AddDevicePage(IIPRespository ipRepository)
        {
            InitializeComponent();

            _ipRepository = ipRepository;
        }

        private void IPField_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            string value = IPField.Text;
            if (string.IsNullOrEmpty(value))
            {
                IPField.Description = string.Empty;
                return;
            } 

            string isValid = ValidateIP(value);
            IPField.Description = isValid;
        }

        private void BtnAddDevice_Click(object sender, RoutedEventArgs e)
        {
            string value = IPField.Text;
            string isValid = ValidateIP(value);
            IPField.Description = isValid;
            if (isValid != string.Empty)
                return;

            InternetProtocol internetProtocol = new InternetProtocol(value);
            _ipRepository.Save(internetProtocol);

            OkDialog.ShowAsync();
            IPField.Text = string.Empty;
        }

        private string ValidateIP(string text)
        {
            bool result = IPAddress.TryParse(text, out _);
            if (result)
                return string.Empty;
            
            if (_ipRepository.GetByIP(text) != null)
                return "Este IP já está sendo utilizado";

            return "O IP é inválido";
        }
    }
}
