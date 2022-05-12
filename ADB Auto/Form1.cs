using ADB_Auto.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADB_Auto
{
    public partial class Form1 : Form
    {
        private readonly DialogService dialogService = new DialogService();

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists("SaveIPs.txt"))
            {
                StreamWriter sw = new StreamWriter("SaveIPs.txt");
                sw.Close();
            }
            FillListIPs();
            PickIpsInReturn();
            Application.EnableVisualStyles();
        }

        private void FillListIPs()
        {
            listBox1.Items.Clear();
            foreach (string line in System.IO.File.ReadAllLines("SaveIPs.txt"))
            {
                listBox1.Items.Add(line);
            }
        }

        private void PickIpsInReturn()
        {

            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.Start();
            cmd.StandardInput.WriteLine("cls");
            cmd.StandardInput.WriteLine("adb devices");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            string result = cmd.StandardOutput.ReadToEnd();

            List<string> ips = new List<string>();

            foreach (string line in System.IO.File.ReadAllLines("SaveIPs.txt"))
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
                MessageBox.Show("Selecione um dispositivo");
                return;
            }

            string ip = listBox1.SelectedItem.ToString().Trim() + ":5555";

            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.Start();

            cmd.StandardInput.WriteLine("adb tcpip 5555");
            cmd.StandardInput.WriteLine("adb connect " + ip);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            string result = cmd.StandardOutput.ReadToEnd();
        }

        private void Disconnect()
        {
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Selecione um dispositivo");
                return;
            }
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.Start();
            cmd.StandardInput.WriteLine("adb disconnect " + listBox2.SelectedItem.ToString());
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            PickIpsInReturn();
        }

        private void SaveIPs()
        {
            if (ipTxt.Text.Length < 7)
            {
                MessageBox.Show("Insira um endereço IP");
                return;
            }

            try
            {
                StreamWriter sw = new StreamWriter("SaveIPs.txt", true);
                sw.WriteLine(ipTxt.Text);
                sw.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Tente novamente");
            }
            finally
            {
                FillListIPs();
                ipTxt.Text = "";
            }
        }

        private void DeleteDevice()
        {
            bool result = dialogService.QuestionDialog("Não é recomendado excluir um dispositivo conectado ou que está conectando", "Deseja excluir este dispositivo?");
            if (!result)
                return;

            List<string> list = new List<string>();

            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Selecione um dispositivo");
                return;
            }

            foreach (string line in System.IO.File.ReadAllLines("SaveIPs.txt"))
            {
                list.Add(line);
            }

            if (list.Contains(listBox1.SelectedItem.ToString()))
            {
                list.Remove(listBox1.SelectedItem.ToString());
            }

            StreamWriter sw = new StreamWriter("SaveIPs.txt");

            foreach (var item in list)
            {
                sw.WriteLine(item);
            }

            sw.Close();

            FillListIPs();
        }

        private async void InstallApk(string path, bool back)
        {
            progressBar1.Visible = true;
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.Start();
            cmd.StandardInput.WriteLine("adb -s " + listBox2.SelectedItem.ToString() + ":5555 " + "install " + path);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            await Task.Delay(3000);

            if (!back)
                MessageBox.Show(cmd.StandardOutput.ReadToEnd());

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
            MessageBoxButtons mb = MessageBoxButtons.YesNo;

            DialogResult result = MessageBox.Show(
                    "Se optar por instalar em segundo plano não terá o feedback quando terminar a instação",
                    "Deseja instalar em segundo plano?",
                    mb
                );

            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Escolha um dispositivo primeiro");
                return;
            }

            if (pathTxt.Text == "")
                return;

            if (result == DialogResult.Yes)
            {
                InstallApk(pathTxt.Text, true);
                return;
            }

            InstallApk(pathTxt.Text, false);
        }

        private void btnKill_Click(object sender, EventArgs e)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.Start();

            cmd.StandardInput.WriteLine("adb start-server");
            cmd.StandardInput.WriteLine("adb kill-server");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            Application.Restart();
        }















        //codigo exemplo
        /*private void button1_Click(object sender, EventArgs e)
        {
             Process cmd = new Process();
             cmd.StartInfo.FileName = "cmd.exe";
             cmd.StartInfo.RedirectStandardInput = true;
             cmd.StartInfo.RedirectStandardOutput = true;
             cmd.StartInfo.CreateNoWindow = true;
             cmd.StartInfo.UseShellExecute = false;

             cmd.Start();

             cmd.StandardInput.WriteLine("adb tcpip 5555");
             cmd.StandardInput.WriteLine("adb connect 192.168.0.2:5555");
             cmd.StandardInput.Flush();
             cmd.StandardInput.Close();

             File.WriteAllText("result.txt", cmd.StandardOutput.ReadToEnd());

            int count = 0;
            string result = "";
            foreach (string line in System.IO.File.ReadAllLines("result.txt"))
            {
                if(count == 7)
                    result = line;
                count++;
            }

            MessageBox.Show(result);

            var array = result.ToArray();
            string numeros = "";
            foreach (var item in array)
            {
                if((int.TryParse(item.ToString(), out int parsed)) || (item == '.') || item == ':')
                {
                    numeros = numeros + item;
                }
            }

            MessageBox.Show(numeros);

        }*/
    }
}
