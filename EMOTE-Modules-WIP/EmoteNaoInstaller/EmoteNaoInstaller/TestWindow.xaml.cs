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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmoteNaoInstaller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        EmoteNaoPackage package;


        public TestWindow()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            RemoteNAO remoteNao = new RemoteNAO(txtAddress.Text, txtUsername.Text, txtPassword.Text, "");
            List<string> commands = new List<string>();
            commands.Add("tar -xvf data.tar.gz; cd /home/nao/InstallBehaviors; python installBehaviors.py; cd ..; rm -R InstallBehaviors; rm data.tar.gz");
            EmoteNaoPackage p = new EmoteNaoPackage("/home/nao/", null, commands);
            p.Create(txtDataFolder.Text,txtOutputPath.Text);
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            package = EmoteNaoPackage.Load(txtPackageToLoadPath.Text);
            Console.WriteLine("ok");
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            RemoteNAO remoteNao = new RemoteNAO(txtAddress.Text, txtUsername.Text, txtPassword.Text, package.WorkingDirectory);
            remoteNao.UploadFile(txtFileToSendPath.Text, null);
        }


        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            RemoteNAO remoteNao = new RemoteNAO(txtAddress.Text, txtUsername.Text, txtPassword.Text, package.WorkingDirectory);
            remoteNao.ExecuteCommand(txtCommand.Text);
        }

        private void btnInstall_Click_1(object sender, RoutedEventArgs e)
        {
            RemoteNAO remoteNao = new RemoteNAO(txtAddress.Text, txtUsername.Text, txtPassword.Text, package.WorkingDirectory);
            EmotePackageManager.Install(remoteNao,package);
        }

    }
}
