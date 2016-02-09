using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Collections.ObjectModel;


namespace EmoteNaoInstaller
{
    public partial class EmotePackageInstaller : Window
    {
        EmoteNaoPackage package;

        ObservableCollection<string> precommands = new ObservableCollection<string>();
        ObservableCollection<string> postcommands = new ObservableCollection<string>();


        public EmotePackageInstaller()
        {
            InitializeComponent();
            EmotePackageManager.LogEvent += EmotePackageManager_LogEvent;
            EmotePackageManager.UploadCompleted += EmotePackageManager_UploadCompleted;
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                string packagePath = args[1];
                if (System.IO.Path.GetExtension(packagePath).Equals("epkg"))
                {
                    package = EmoteNaoPackage.Load(packagePath);
                    UpdatePackageInfo();
                }
            }
            lstPreInstallationCommandsPkgCreation.ItemsSource = precommands;
            lstPostInstallationCommandsPkgCreation.ItemsSource = postcommands;

            postcommands.Add("tar -xvf data.tar.gz;");
            postcommands.Add("rm data.tar.gz");

        }

        void EmotePackageManager_UploadCompleted(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                txtUploadMessage.Text = "Upload completed";
                prgUploadBar.Value = 0;
                prgUploadBar.IsEnabled = false;
            }));
        }

        void EmotePackageManager_LogEvent(object sender, EmotePackageManager.LogEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (e.IsFileTransfer)
                {
                    prgUploadBar.IsEnabled = true;
                    txtUploadMessage.Text = e.Message;
                    try
                    {
                        prgUploadBar.Value = e.FileTransferCompletition * 100;
                    }
                    catch (System.ArgumentException ex) { 
                        // Sometimes the upload doesn't go well, so you need to stop the upload task and inform the user or retry
                        Application.Current.Shutdown();
                    }
                }
                else
                {
                    Log(e.Message);
                }
            }));
        }

        private void btnOpenPackage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Emote package install file|*.epkg";
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string path = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                package = EmoteNaoPackage.Load(path);
                UpdatePackageInfo();
            }
        }

        private void UpdatePackageInfo()
        {
            lstPostInstallationCommands.ItemsSource = package.PostInstallCommands;
            lstPreInstallationCommands.ItemsSource = package.PreInstallCommands;
            tblPackageName.Text = package.Name;
        }

        private async void btnInstallPackage_Click(object sender, RoutedEventArgs e)
        {
            var remote = new RemoteNAO(txtAddress.Text, txtUsername.Text, txtPassword.Text, package.WorkingDirectory); 
            Log("Installation started, please wait..");
            btnInstallPackage.IsEnabled = false;
            await EmotePackageManager.InstallAsync(remote,package);
            btnInstallPackage.IsEnabled = true;
        }

        private void btnTestWindow_Click(object sender, RoutedEventArgs e)
        {
            TestWindow testWindow = new TestWindow();
            testWindow.Show();
        }

        private void Log(string message)
        {
            txtInstallationLog.Text = txtInstallationLog.Text + message + Environment.NewLine;
            txtInstallationLog.ScrollToEnd();
        }

        private async void btnRunPythonClient_Click(object sender, RoutedEventArgs e)
        {
            btnRunPythonClient.IsEnabled = false;
            btnStopRunPythonClient.IsEnabled = true;
            btnRunPythonClient.Content = "running...";
            RemoteNAO remote = new RemoteNAO(txtAddress.Text, txtUsername.Text, txtPassword.Text, "/home/nao/");
            await remote.ExecuteCommandAsync( "python NAOThalamus/naoqiXmlrpc.py > Log");
            btnRunPythonClient.IsEnabled = true;
            btnRunPythonClient.Content = "Run Python Client";
            btnStopRunPythonClient.IsEnabled = false;
        }

        private void btnStopRunPythonClient_Click(object sender, RoutedEventArgs e)
        {
            RemoteNAO remote = new RemoteNAO(txtAddress.Text, txtUsername.Text, txtPassword.Text, "/home/nao/");
            remote.ExecuteCommand(@"kill -9 $(ps -A | grep python2.7 | awk '{print $1;}')");
        }

        private void btnSelectSourceFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            openFolderDialog.Description = "Select the folder cotaining the source files";
            System.Windows.Forms.DialogResult result = openFolderDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtDataFolder.Text = openFolderDialog.SelectedPath;
            }
            lstFilesToPack.ItemsSource = Directory.GetFiles(openFolderDialog.SelectedPath);
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            openFolderDialog.Description = "Select the folder where to create the package";
            System.Windows.Forms.DialogResult result = openFolderDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                EmoteNaoPackage p = new EmoteNaoPackage("/home/nao/", precommands.ToList<string>(), postcommands.ToList<string>());

                p.Create(txtDataFolder.Text, openFolderDialog.SelectedPath + "\\" + txtPackageName.Text);
            }
        }

        private void btnAddPre_Click(object sender, RoutedEventArgs e)
        {
            if (txtNewCommand.Text != "")
            {
                precommands.Add(txtNewCommand.Text);
            }
        }

        private void btnAddPost_Click(object sender, RoutedEventArgs e)
        {
            if (txtNewCommand.Text != "")
            {
                postcommands.Add(txtNewCommand.Text);
            }
        }

        private void btnRemovePreCommand_Click(object sender, RoutedEventArgs e)
        {
            if (lstPreInstallationCommandsPkgCreation.SelectedItem != null)
            {
                precommands.RemoveAt(lstPreInstallationCommandsPkgCreation.SelectedIndex);
            }
        }

        private void btnRemovePostCommand_Click(object sender, RoutedEventArgs e)
        {
            if (lstPostInstallationCommandsPkgCreation.SelectedItem != null)
            {
                postcommands.RemoveAt(lstPostInstallationCommandsPkgCreation.SelectedIndex);
            }
        }




    }
}
