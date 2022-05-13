using ADB_Auto.Models;
using ADB_Auto.Repositories;
using ADB_Auto.Repositories.Interfaces;
using ADB_Auto.Services;
using ADB_Auto.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADB_Auto
{
    public partial class Form1 : Form
    {
        private readonly DialogService dialogService = new DialogService();
        private readonly IIPRespository ipRepository;
        private readonly IADBService adbService;
        private readonly BindingList<InternetProtocol> savedIPs;

        public Form1()
        {
            InitializeComponent();

            ipRepository = new IPRepository();
            adbService = new ADBService();

            savedIPs = new BindingList<InternetProtocol>(ipRepository.GetAll());
            listBox1.DataSource = savedIPs;
            listBox1.DisplayMember = "IP";
            listBox1.ValueMember = "IP";

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PickIpsInReturn();
            Application.EnableVisualStyles();
        }

        private void PickIpsInReturn()
        {
            string result = adbService.GetAllConnectedDevices();

            List<string> ips = new List<string>();
            foreach (InternetProtocol line in savedIPs)
            {
                if (result.Contains(line.IP))
                {
                    ips.Add(line.IP);
                }
            }

            listBox2.Items.Clear();
            foreach (var item in ips)
            {
                listBox2.Items.Add(item);
            }
        }

        private void Connect()
        {
            if (listBox1.SelectedItem == null)
            {
                dialogService.ShowMessage("Selecione um dispositivo");
                return;
            }

            string ip = listBox1.SelectedItem.ToString().Trim() + ":5555";
            adbService.ConnectToDevice(ip);
        }

        private void Disconnect()
        {
            if (listBox2.SelectedItem == null)
            {
                dialogService.ShowMessage("Selecione um dispositivo");
                return;
            }

            adbService.DisconnectFromDevice(listBox2.SelectedItem.ToString());
            PickIpsInReturn();
        }

        private void SaveIPs()
        {
            string ip = ipTxt.Text;
            if (!IPAddress.TryParse(ip, out _))
            {
                dialogService.ShowMessage("Insira um endereço IP válido");
                return;
            }

            InternetProtocol internetProtocol = new InternetProtocol(ip);

            bool isSaved = ipRepository.Save(internetProtocol);
            if (!isSaved)
            {
                dialogService.ShowMessage("Tente novamente");
                return;
            }

            savedIPs.Add(internetProtocol);
            ipTxt.Text = "";
        }

        private void DeleteDevice()
        {
            bool result = dialogService.QuestionDialog("Não é recomendado excluir um dispositivo conectado ou que está conectando", "Deseja excluir este dispositivo?");
            if (!result)
                return;

            if (listBox1.SelectedItem == null)
            {
                dialogService.ShowMessage("Selecione um dispositivo");
                return;
            }

            int index = listBox1.SelectedIndex;
            savedIPs.RemoveAt(index);
            ipRepository.Remove(savedIPs);
        }

        private async void InstallApk(string path, bool back)
        {
            progressBar1.Visible = true;
            string message = adbService.InstallApk(listBox2.SelectedItem.ToString(), path, back);

            await Task.Delay(3000);

            if (!back)
                dialogService.ShowMessage(message);

            progressBar1.Visible=false;
        }

        private void BtnAddIP_Click(object sender, EventArgs e)
        {
            SaveIPs();
        }

        private void conBtn_Click(object sender, EventArgs e)
        {
            Connect();
            PickIpsInReturn();
        }

        private void discBtn_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void ipTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeleteDevice();
        }

        private void BtnSearchAPK_Click(object sender, EventArgs e)
        {
            string path = dialogService.OpenAPKFile();
            pathTxt.Text = path;
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            bool result = dialogService.QuestionDialog("Se optar por instalar em segundo plano não terá o feedback quando terminar a instação", "Deseja instalar em segundo plano?");

            if (listBox2.SelectedItem == null)
            {
                dialogService.ShowMessage("Escolha um dispositivo primeiro");
                return;
            }

            if (pathTxt.Text == "")
                return;

            if (result)
            {
                InstallApk(pathTxt.Text, true);
                return;
            }

            InstallApk(pathTxt.Text, false);
        }

        private void BtnKill_Click(object sender, EventArgs e)
        {
            adbService.KillServer();
            Application.Restart();
        }

    }
}
