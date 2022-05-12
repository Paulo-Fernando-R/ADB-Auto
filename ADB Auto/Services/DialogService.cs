using System.Windows.Forms;

namespace ADB_Auto.Services
{
    public class DialogService
    {
        public string OpenAPKFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "APK files (*.apk)|*.apk|All files(*.*)|*.*";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }

            return string.Empty;
        }
    }
}
