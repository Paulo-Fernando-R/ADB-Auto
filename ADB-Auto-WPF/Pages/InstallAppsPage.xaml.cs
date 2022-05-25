using ADB.Core.Models;
using ADB.Core.Repositories.Interfaces;
using ADB.Core.Services.Interfaces;
using ADB_Auto_WPF.Services;
using ModernWpf.Controls;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace ADB_Auto_WPF.Pages
{
    /// <summary>
    /// Interaction logic for InstallAppsPage.xaml
    /// </summary>
    public partial class InstallAppsPage : System.Windows.Controls.Page
    {
        private readonly IADBService _adbService;
        private readonly IIPRespository _ipRepository;
        private readonly DialogService _dialogService;
        private IList<InternetProtocol> ips;
        private string filePath;

        public InstallAppsPage(IIPRespository ipRepository, IADBService adbService)
        {
            InitializeComponent();

            _adbService = adbService;
            _ipRepository = ipRepository;
            _dialogService = new DialogService();
            ips = new List<InternetProtocol>();
            filePath = null;
        }

        private void BtnChooseAPK_Click(object sender, RoutedEventArgs e)
        {
            TxtFile.Description = string.Empty;
            string fileName = _dialogService.OpenFileDialog();
            if (fileName == null)
            {
                TxtFile.Description = "Escolha um APK";
                return;
            }

            filePath = fileName;
            TxtFile.Text = Path.GetFileNameWithoutExtension(filePath);
        }

        private void CbxIP_Loaded(object sender, RoutedEventArgs e)
        {
            string connectedIPs = _adbService.GetAllConnectedDevices();
            IList<InternetProtocol> savedIPs = _ipRepository.GetAll();

            foreach (InternetProtocol savedIP in savedIPs)
            {
                if (connectedIPs.Contains(savedIP.IP))
                {
                    ips.Add(savedIP);
                    CbxIP.Items.Add(savedIP.ToString());
                }
            }
        }

        private async void BtnInstall_Click(object sender, RoutedEventArgs e)
        {
            TxtFile.Description = string.Empty;

            if (filePath == null)
            {
                TxtFile.Description = "Escolha um APK";
                return;
            }

            int selectedIndex = CbxIP.SelectedIndex;
            if (selectedIndex == -1)
            {
                await ChooseIPDialog.ShowAsync();
                return;
            }

            ContentDialogResult result = await QuestionDialog.ShowAsync();
            bool back = result == ContentDialogResult.Primary;

            InternetProtocol selectedIP = ips[selectedIndex];
            string message = _adbService.InstallApk(selectedIP.IP, filePath, back);

            await Task.Delay(3000);

            if (!back)
            {
                SuccessDialog.Content = message;
                await SuccessDialog.ShowAsync();
            }
        }

    }
}
