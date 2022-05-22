using ADB.Core.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ADB_Auto_WPF.Components
{
    /// <summary>
    /// Interaction logic for IPController.xaml
    /// </summary>
    public partial class IPController : UserControl
    {
        public event RoutedEventHandler OnDelete;

        private readonly InternetProtocol _internetProtocol;

        public IPController(InternetProtocol internetProtocol)
        {
            InitializeComponent();

            _internetProtocol = internetProtocol;
            IPField.Text = internetProtocol.IP;
        }

        private void FontIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnDelete?.Invoke(_internetProtocol, e);
        }
    }
}
