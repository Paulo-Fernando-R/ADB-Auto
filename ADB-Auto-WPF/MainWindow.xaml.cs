using ADB.Core.Repositories;
using ADB.Core.Services;
using ADB.Core.Services.Interfaces;
using ADB_Auto_WPF.Pages;
using ModernWpf.Controls;
using System;
using System.IO;
using System.Windows;

namespace ADB_Auto_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IPRepository _ipRepository;
        private readonly IADBService _adbService;

        public MainWindow()
        {
            InitializeComponent();

            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folderPath = string.Format("{0}\\ADBTools", appData);
            string filePath = string.Format("{0}\\SaveIPs.txt", folderPath);
            Directory.CreateDirectory(folderPath);

            _ipRepository = new IPRepository(filePath);
            _adbService = new ADBService();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NavView.SelectedItem = NavView.MenuItems[0];
            NavView_Navigate("AddDevice");
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            string tag = args.SelectedItemContainer.Tag.ToString();

            NavView_Navigate(tag);
        }

        private void NavView_Navigate(string navItemTag)
        {
            while (ContentFrame.NavigationService.RemoveBackEntry() != null) ;

            if (navItemTag.Equals("AddDevice"))
            {
                ContentFrame.Navigate(new AddDevicePage(_ipRepository));
            }
            else if (navItemTag.Equals("SavedDevices"))
            {
                ContentFrame.Navigate(new SavedDevicesPage(_ipRepository, _adbService));
            }
            else if (navItemTag.Equals("InstallApps"))
            {
                ContentFrame.Navigate(new InstallAppsPage(_ipRepository, _adbService));
            }
            else if (navItemTag.Equals("About"))
            {
                ContentFrame.Navigate(new AboutPage());
            }
        }
    }
}
