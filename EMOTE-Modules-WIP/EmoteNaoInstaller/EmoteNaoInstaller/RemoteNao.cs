using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Renci.SshNet;
using System.IO;
using System.Threading.Tasks;

namespace EmoteNaoInstaller
{
    class RemoteNAO
    {
        string host;
        string username;
        string password;
        public string Workingdirectory { get; private set; }
        public SshClient SSH { get; private set; }

        public IAsyncResult asynch { get; private set; }

        public RemoteNAO(string host, string username, string password, string workingdirectory)
        {
            this.host = host;
            this.username = username;
            this.password = password;
            this.Workingdirectory = workingdirectory;
        }

        private ConnectionInfo GenerateConnectionInfo()
        {
            var auth1 = new PasswordAuthenticationMethod(username, password);
            var auth2 = new KeyboardInteractiveAuthenticationMethod(username);
            auth2.AuthenticationPrompt += auth2_AuthenticationPrompt;
            ConnectionInfo ci = new ConnectionInfo(host, username, auth1, auth2);
            return ci;
        }

        void auth2_AuthenticationPrompt(object sender, Renci.SshNet.Common.AuthenticationPromptEventArgs e)
        {
            foreach (var prompt in e.Prompts)
            {
                if (prompt.Request.Equals("Password: ", StringComparison.InvariantCultureIgnoreCase))
                {
                    prompt.Response = password; // "Password" acquired from resource
                }
            }
        }

        public void Connect()
        {
            SSH = new SshClient(GenerateConnectionInfo());
            SSH.ErrorOccurred += ssh_ErrorOccurred;
            SSH.Connect();
        }

        public void Disconnect()
        {
            SSH.Disconnect();
        }

        public Task<bool> UploadFileAsync(string localFileName, Action<ulong> uploadCallback)
        {
            return Task.Run<bool>(() => UploadFile(localFileName, uploadCallback));
        }

        public bool UploadFile(string localFileName, Action<ulong> uploadCallback)
        {
            string remoteFileName = System.IO.Path.GetFileName(localFileName);

            using (var sftp = new SftpClient(GenerateConnectionInfo()))
            {
                sftp.Connect();
                //TODO: check if the directory exists!
                sftp.ChangeDirectory(Workingdirectory);
                sftp.ErrorOccurred += ssh_ErrorOccurred;

                using (var file = File.OpenRead(localFileName))
                {
                    try
                    {
                        sftp.UploadFile(file, remoteFileName, uploadCallback);
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }

                sftp.Disconnect();
            }
            return true;
        }

        public Task<SshCommand> ExecuteCommandAsync(string command)
        {
            return Task.Run<SshCommand>(() => ExecuteCommand(command));
        }

        public SshCommand ExecuteCommand(string command)
        {
            Connect();
            string workingDirectoryCommand = "cd " + Workingdirectory + "; ";
            var sshcommand = SSH.RunCommand(workingDirectoryCommand + command);
            Disconnect();
            return sshcommand;
        }

        public Task<SshCommand> ExecuteCommand2Async(string command)
        {
            return Task.Run<SshCommand>(() => ExecuteCommand2(command));
        }

        public SshCommand ExecuteCommand2(string command)
        {
            SSH.Connect();
            var cmd = SSH.CreateCommand("sleep 30s;date"); 
            asynch = cmd.BeginExecute(null, null);
            
            while (!(asynch.IsCompleted))
            {
                Console.WriteLine("Waiting for commands to complete...");
                System.Threading.Thread.Sleep(2000);
            }
            cmd.EndExecute(asynch);
            SSH.Disconnect();
            return cmd;
        }
       
        void ssh_ErrorOccurred(object sender, Renci.SshNet.Common.ExceptionEventArgs e)
        {
            throw new Exception("SSH Error", e.Exception);
        }
        
    }
    

    //public class RemoteCommand
    //{
    //    public SshCommand Command { get; private set; }

    //    IAsyncResult asynch;

    //    public RemoteCommand(SshCommand command)
    //    {
    //        Command = command;
    //    }

    //    public void Execute(){
    //        asynch = Command.BeginExecute();
    //    }

    //    public void Stop()
    //    {
    //        Command.EndExecute(asynch);
    //    }
    //}

    
}
