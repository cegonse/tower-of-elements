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
        private int _currentFile = 1;
        private SynchronizationResult _synchronizationResult;
        private Thread _t;

        public SyncForm(MainForm mf)
        {
            InitializeComponent();
            _main = mf;
        }

        private void SyncForm_Load(object sender, EventArgs e)
        {
            _t = new Thread(StartTrasfer);
            _t.Start();
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
                        session.SynchronizeDirectories(SynchronizationMode.Both, _main.GetSettings().ResourcesPath,
                        "/home/"+_main.GetSettings().SSHUsername+"/Icegame/Resources", false, false, SynchronizationCriteria.Time);
                }
            }
            catch { }
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

                if (_synchronizationResult == null)
                {
                    MessageBox.Show(_currentFile + " ficheros sincronizados.", "Sincronización Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Invoke(new Action(() =>
                    {
                        Close();
                    }));

                    _t.Join();
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
            try
            {
                Invoke(new Action(() =>
                {
                    progressBarOverall.Value = (int)(e.OverallProgress * 100.0);
                    labelOverall.Text = (e.OverallProgress * 100.0).ToString() + "%";

                    progressBarFile.Value = (int)(e.FileProgress * 100.0);
                    labelFile.Text = "Comprobando: " + e.FileName;
                }));
            }
            catch { }
        }
    }
}
