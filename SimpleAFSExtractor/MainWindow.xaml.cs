using AFSLib;
using System.IO;
using System.Linq;
using System.Windows;

namespace SimpleAFSExtractor {

    public partial class MainWindow : Window {

        AfsArchive currentAfs;

        public MainWindow() {
            InitializeComponent();
        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e) {
            var dialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog
            {
                Filter = "AFS files (*.afs)|*.afs|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == false) {
                return;
            }
            if (!dialog.FileName.ToLower().EndsWith(".afs")) {
                MessageBox.Show("Pick an 'AFS' file", "Try Again");
                return;
            }
            var data = File.ReadAllBytes(dialog.FileName);
            if (AfsArchive.TryFromFile(data, out var afsArchive)) {
                currentAfs = afsArchive;
                filePicked.Content = Path.GetFileName(dialog.FileName);
                extractButton.IsEnabled = true;
            };
        }

        private void ButtonExtract_Click(object sender, RoutedEventArgs e) {
            if (currentAfs == null)
                return;
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog() == false) {
                return;
            }
            if (dialog.SelectedPath != "") {
                if (indexExportCheckbox.IsChecked.Value) {
                    for (int i = 0; i < currentAfs.Files.Count(); i++) {
                        File.WriteAllBytes(dialog.SelectedPath + "\\\\" + i + "_" + currentAfs.Files[i].Name, currentAfs.Files[i].Data);
                    }
                } else {
                    for (int i = 0; i < currentAfs.Files.Count(); i++) {
                        File.WriteAllBytes(dialog.SelectedPath + "\\\\" + currentAfs.Files[i].Name, currentAfs.Files[i].Data);
                    }
                }
                MessageBox.Show("Extraction completed", "Simple AFS Extractor");
            }
        }
    }
}
