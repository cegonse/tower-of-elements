using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.IO;

namespace IceGameEditor
{
    public partial class RemoteDebug : Form
    {
        private NetworkInterface _serverInterface = null;
        private NetworkInterface[] _interfaces = null;

        public RemoteDebug()
        {
            InitializeComponent();
            _interfaces = NetworkInterface.GetAllNetworkInterfaces();
        }

        public NetworkInterface[] GetInterfaceList()
        {
            return _interfaces;
        }

        public void StartServer(int index)
        {
            if (index >= 0)
            {
                _serverInterface = _interfaces[index];
            }

            labelAddress.Text += _serverInterface.GetIPProperties().UnicastAddresses[0].Address.ToString();

            System.Net.HttpListener newHttpListener = new System.Net.HttpListener();
            newHttpListener.Prefixes.Add("http://" + _serverInterface.GetIPProperties().UnicastAddresses[0].Address.ToString() + ":8123/");
            newHttpListener.Start();

            System.Net.HttpListenerContext context = newHttpListener.GetContext();
            byte[] byteArr = System.Text.ASCIIEncoding.ASCII.GetBytes(File.ReadAllText("data/tmp/debuglevel.txt"));
            context.Response.OutputStream.Write(byteArr, 0, byteArr.Length);
            context.Response.Close();

            newHttpListener.Stop();
        }

        public void Init()
        {
            if (_interfaces.Length == 1)
            {
                _serverInterface = _interfaces[0];
                StartServer(-1);
            }
            else
            {
                InterfaceSelector isf = new InterfaceSelector(this);
                isf.Show();
                isf.Init();
            }
        }

        public void StopServer()
        {

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            RemoteDebug_FormClosing(null, null);
            this.Close();
        }

        private void RemoteDebug_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopServer();
        }
    }
}
