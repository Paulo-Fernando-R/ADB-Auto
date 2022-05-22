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
        public static readonly DependencyProperty ShowPrimaryButtonDependency = DependencyProperty.Register("ShowPrimaryButton", typeof(Visibility), typeof(IPController), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty ShowSecondaryButtonDependency = DependencyProperty.Register("ShowSecondaryButton", typeof(Visibility), typeof(IPController), new PropertyMetadata(Visibility.Visible));

        public Visibility ShowPrimaryButton
        {
            get => (Visibility)GetValue(ShowPrimaryButtonDependency);
            set => SetValue(ShowPrimaryButtonDependency, value);
        }

        public Visibility ShowSecondaryButton
        {
            get => (Visibility)GetValue(ShowSecondaryButtonDependency);
            set => SetValue(ShowSecondaryButtonDependency, value);
        }

        public event RoutedEventHandler OnPrimaryButton;
        public event RoutedEventHandler OnSecondaryButton;

        private readonly InternetProtocol _internetProtocol;

        public IPController(InternetProtocol internetProtocol)
        {
            InitializeComponent();

            _internetProtocol = internetProtocol;
            IPField.Text = internetProtocol.IP;
        }

        private void PrimaryButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnPrimaryButton?.Invoke(_internetProtocol, e);
        }

        private void SecondaryButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnSecondaryButton?.Invoke(_internetProtocol, e);
        }

    }
}
