using ADB.Core.Models;
using ADB.Core.Repositories.Interfaces;
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
        private IList<InternetProtocol> _internetProtocols;

        public SavedDevicesPage(IIPRespository ipRepository)
        {
            InitializeComponent();

            _ipRepository = ipRepository;
        }

        private void PanelSavedDevices_Loaded(object sender, RoutedEventArgs e)
        {
            _internetProtocols = _ipRepository.GetAll();
            foreach (InternetProtocol internetProtocol in _internetProtocols)
            {
                IPController ipController = new IPController(internetProtocol);
                ipController.OnDelete += IpController_OnDelete;
                PanelSavedDevices.Children.Add(ipController);
            }
        }

        private async void IpController_OnDelete(object sender, RoutedEventArgs e)
        {
            ContentDialogResult dialogResult = await QuestionDialog.ShowAsync();
            if (dialogResult != ContentDialogResult.Primary)
                return;

            InternetProtocol internetProtocol = sender as InternetProtocol;
            int index = _internetProtocols.IndexOf(internetProtocol);

            _internetProtocols.RemoveAt(index);
            PanelSavedDevices.Children.RemoveAt(index);

            _ipRepository.Remove(internetProtocol);
        }
    }
}
