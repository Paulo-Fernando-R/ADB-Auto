using ADB.Core.Models;
using ADB.Core.Repositories.Interfaces;
using ADB.Core.Services.Interfaces;
using ADB_Auto_WPF.Components;
using ModernWpf.Controls;
using System.Collections.Generic;
using System.Windows;

namespace ADB_Auto_WPF.Pages
{
    /// <summary>
    /// Interaction logic for SavedDevicesPage.xaml
    /// </summary>
    public partial class SavedDevicesPage : System.Windows.Controls.Page
    {
        private readonly IIPRespository _ipRepository;
        private readonly IADBService _adbService;
        private IList<InternetProtocol> _savedDevices;
        private IList<InternetProtocol> _connectedDevices;

        public SavedDevicesPage(IIPRespository ipRepository, IADBService adbService)
        {
            InitializeComponent();

            _ipRepository = ipRepository;
            _adbService = adbService;
        }

        private void PanelSavedDevices_Loaded(object sender, RoutedEventArgs e)
        {
            _savedDevices = _ipRepository.GetAll();
            foreach (InternetProtocol savedIP in _savedDevices)
            {
                IPController ipController = new IPController(savedIP);
                ipController.OnSecondaryButton += SavedIP_OnSecondaryButton;
                PanelSavedDevices.Children.Add(ipController);
            }
        }

        private async void SavedIP_OnSecondaryButton(object sender, RoutedEventArgs e)
        {
            ContentDialogResult dialogResult = await RemoveIPDialog.ShowAsync();
            if (dialogResult != ContentDialogResult.Primary)
                return;

            InternetProtocol internetProtocol = sender as InternetProtocol;
            int index = _savedDevices.IndexOf(internetProtocol);

            _savedDevices.RemoveAt(index);
            PanelSavedDevices.Children.RemoveAt(index);

            _ipRepository.Remove(internetProtocol);
        }

        private void PanelConnectedDevices_Loaded(object sender, RoutedEventArgs e)
        {
            LoadConnectedDevices();
        }

        private void LoadConnectedDevices()
        {
            _connectedDevices = new List<InternetProtocol>();
            PanelConnectedDevices.Children.Clear();
            string ips = _adbService.GetAllConnectedDevices();
        
            foreach (InternetProtocol savedDevice in _savedDevices)
            {
                if (!ips.Contains(savedDevice.IP))
                    continue;
                
                _connectedDevices.Add(savedDevice);

                IPController connectedIP = new IPController(savedDevice);
                connectedIP.OnSecondaryButton += ConnectedIP_OnSecondaryButton;
                connectedIP.ShowPrimaryButton = Visibility.Hidden;
                PanelConnectedDevices.Children.Add(connectedIP);
            }
        }

        private async void ConnectedIP_OnSecondaryButton(object sender, RoutedEventArgs e)
        {
            ContentDialogResult dialogResult = await DisconnectIPDialog.ShowAsync();
            if (dialogResult != ContentDialogResult.Primary)
                return;

            InternetProtocol connectedDevice = sender as InternetProtocol;
            int index = _connectedDevices.IndexOf(connectedDevice);

            _adbService.DisconnectFromDevice(connectedDevice.IP);
            _connectedDevices.Remove(connectedDevice);
            PanelConnectedDevices.Children.RemoveAt(index);
        }
    }
}
