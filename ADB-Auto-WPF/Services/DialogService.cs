using Microsoft.Win32;
using System;

namespace ADB_Auto_WPF.Services
{
    public class DialogService
    {
        public string OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Multiselect = false,
                Filter = "APK files (*.apk)|*.apk",
                FilterIndex = 0,
                Title = "Escolha um APK"
            };
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string fileName = openFileDialog.FileName;
                return fileName;
            }

            return null;
        }
    }
}
