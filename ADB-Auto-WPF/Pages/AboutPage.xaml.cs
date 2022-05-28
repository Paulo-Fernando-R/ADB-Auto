using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace ADB_Auto_WPF.Pages
{
    /// <summary>
    /// Interaction logic for AboutPage.xaml
    /// </summary>
    public partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();

            string version = Assembly.GetEntryAssembly().GetName().Version.ToString();
            TxtVersion.Text = string.Format("Versão: {0}", version);
        }

        private void BtnAlessandra_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.linkedin.com/in/alessandra-diamantino-85b25a191");
        }

        private void BtnMatheus_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.linkedin.com/in/theu-ferreira/");
        }

        private void BtnPaulo_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.linkedin.com/in/paulo-fernando-071bb31a9");
        }
    }
}
