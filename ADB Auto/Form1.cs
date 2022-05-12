using ADB_Auto.Repositories;
using ADB_Auto.Repositories.Interfaces;
using ADB_Auto.Services;
using ADB_Auto.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADB_Auto
{
    public partial class Form1 : Form
    {
        private readonly DialogService dialogService = new DialogService();
        private readonly IIPRespository ipRepository;
        private readonly IADBService adbService;

        public Form1()
        {
            InitializeComponent();

            ipRepository = new IPRepository();
            adbService = new ADBService();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FillListIPs();
            PickIpsInReturn();
            Application.EnableVisualStyles();
        }

        private void FillListIPs()
        {
            listBox1.Items.Clear();

            IList<string> lines = ipRepository.GetAll();
            foreach (string line in lines)
            {
                listBox1.Items.Add(line);
            }
        }

        private void PickIpsInReturn()
        {
            string result = adbService.GetAllConnectedDevices();

            List<string> ips = new List<string>();
            IList<string> lines = ipRepository.GetAll();
            foreach (string line in lines)
            {
                if (result.Contains(line))
                {
                    ips.Add(line);
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

            string result = adbService.ConnectToDevice(ip);
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
            if (ipTxt.Text.Length < 7)
            {
                dialogService.ShowMessage("Insira um endereço IP");
                return;
            }

            bool isSaved = ipRepository.Save(ipTxt.Text);
            if (!isSaved)
            {
                dialogService.ShowMessage("Tente novamente");
                return;
            }

            FillListIPs();
            ipTxt.Text = "";
        }

        private void DeleteDevice()
        {
            bool result = dialogService.QuestionDialog("Não é recomendado excluir um dispositivo conectado ou que está conectando", "Deseja excluir este dispositivo?");
            if (!result)
                return;

            List<string> list = new List<string>();

            if (listBox1.SelectedItem == null)
            {
                dialogService.ShowMessage("Selecione um dispositivo");
                return;
            }

            IList<string> lines = ipRepository.GetAll();
            foreach (string line in lines)
            {
                list.Add(line);
            }

            if (list.Contains(listBox1.SelectedItem.ToString()))
            {
                list.Remove(listBox1.SelectedItem.ToString());
            }

            ipRepository.Remove(list);
            FillListIPs();
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

        private void button1_Click_1(object sender, EventArgs e)
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

        private void label1_Click(object sender, EventArgs e)
        {

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
