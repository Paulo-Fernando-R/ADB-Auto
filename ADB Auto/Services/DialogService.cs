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

        public bool QuestionDialog(string message, string caption)
        {
            MessageBoxButtons mb = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, mb);

            if (result == DialogResult.No)
            {
                return false;
            }

            return true;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
