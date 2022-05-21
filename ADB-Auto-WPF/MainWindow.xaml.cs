using ADB.Core.Repositories;
using ADB_Auto_WPF.Pages;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ADB_Auto_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type page)>
        {
            ("AddDevice", typeof(AddDevicePage)),
            ("SavedDevices", typeof(SavedDevicesPage)),
            ("InstallApps", typeof(InstallAppsPage)),
            ("About", typeof(AboutPage)),
        };

        private readonly IPRepository _ipRepository;

        public MainWindow()
        {
            InitializeComponent();

            _ipRepository = new IPRepository();
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
                ContentFrame.Navigate(new SavedDevicesPage());
            }
            else
            {
                (string Tag, Type Page) item = _pages.FirstOrDefault(p => p.Tag.Equals(navItemTag));
                Type _page = item.Page;

                ContentFrame.Navigate(_page, null);
            }
        }
    }
}
