namespace ADB_Auto.Services.Interfaces
{
    public interface IADBService
    {
        string GetAllConnectedDevices();
        void KillServer();
        string InstallApk(string deviceIP, string path, bool back);
        string ConnectToDevice(string deviceIP);
        void DisconnectFromDevice(string deviceIP);
    }
}
