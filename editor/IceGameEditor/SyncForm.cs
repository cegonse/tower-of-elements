using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using WinSCP;

namespace IceGameEditor
{
    public partial class SyncForm : Form
    {
        private MainForm _main;
        private int _fileCount;
        private int _currentFile = 1;
        private SynchronizationResult _synchronizationResult;

        public SyncForm(MainForm mf)
        {
            InitializeComponent();
            _main = mf;
        }

        private void SyncForm_Load(object sender, EventArgs e)
        {
            Thread t = new Thread(StartTrasfer);
            t.Start();
        }

        private void StartTrasfer()
        {
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Scp,
                    HostName = _main.GetSettings().SSHAddress,
                    UserName = _main.GetSettings().SSHUsername,
                    Password = _main.GetSettings().SSHPassword,
                    GiveUpSecurityAndAcceptAnySshHostKey = true
                };

                using (Session session = new Session())
                {
                    // Will continuously report progress of synchronization
                    session.FileTransferred += FileTransferred;
                    session.FileTransferProgress += FileTransferProgress;

                    // Connect
                    session.Open(sessionOptions);

                    Invoke(new Action(() =>
                    {
                        labelFile.Text = "";
                    }));

                    // Synchronize files
                    _synchronizationResult =
                        session.SynchronizeDirectories(SynchronizationMode.Remote, _main.GetSettings().ResourcesPath,
                        "/home/jumble/Icegame/Resources", false, false, SynchronizationCriteria.Either);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al sincronizar: " + ex.Message, "Error de SCP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                Invoke(new Action(() =>
                {
                    Close();
                }));
            }
        }

        private void FileTransferred(object sender, TransferEventArgs e)
        {
            if (e.Error == null)
            {
                _currentFile++;

                Invoke(new Action(() =>
                {
                    labelTotal.Text = "Progreso total: " + _currentFile.ToString() + " ficheros.";
                }));

                if (_synchronizationResult.IsSuccess)
                {
                    Invoke(new Action(() =>
                    {
                        Close();
                    }));
                }
            }
            else
            {
                MessageBox.Show("Error al sincronizar el archivo" + e.FileName + ": " + e.Error, "Error de SCP", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Invoke(new Action(() =>
                {
                    Close();
                }));
            }
        }

        private void FileTransferProgress(object sender, FileTransferProgressEventArgs e)
        {
            Invoke(new Action(() =>
            {
                progressBarFile.Value = (int)(e.FileProgress * 100.0);
                labelFile.Text = e.FileName;
            }));
        }
    }
}
