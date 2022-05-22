using ADB.Core.Services.Interfaces;
using System.Diagnostics;

namespace ADB.Core.Services
{
    public class ADBService : IADBService
    {
        public string ConnectToDevice(string ip)
        {
            Process cmd = StartCMD();

            cmd.StandardInput.WriteLine("adb tcpip 5555");
            cmd.StandardInput.WriteLine("adb connect " + ip);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            string message = cmd.StandardOutput.ReadToEnd();
            return message;
        }

        public void DisconnectFromDevice(string ip)
        {
            Process cmd = StartCMD();

            cmd.StandardInput.WriteLine("adb disconnect " + ip);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
        }

        public string GetAllConnectedDevices()
        {
            Process cmd = StartCMD();

            cmd.StandardInput.WriteLine("cls");
            cmd.StandardInput.WriteLine("adb devices");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            string message = cmd.StandardOutput.ReadToEnd();
            return message;
        }

        public string InstallApk(string ip, string path, bool back)
        {
            Process cmd = StartCMD();

            cmd.StandardInput.WriteLine("adb -s " + ip + ":5555 " + "install " + path);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            string message = cmd.StandardOutput.ReadToEnd();
            return message;
        }

        public void KillServer()
        {
            Process cmd = StartCMD();

            cmd.StandardInput.WriteLine("adb start-server");
            cmd.StandardInput.WriteLine("adb kill-server");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
        }

        private Process StartCMD()
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.Start();

            return cmd;
        }
    }
}
