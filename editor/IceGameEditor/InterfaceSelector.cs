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
    public partial class InterfaceSelector : Form
    {
        RemoteDebug _interface;
        NetworkInterface[] _interfaces = null;

        public InterfaceSelector(RemoteDebug inter)
        {
            InitializeComponent();
            _interface = inter;
        }

        public void Init()
        {
            _interfaces = _interface.GetInterfaceList();

            foreach (NetworkInterface iff in _interfaces)
            {
                listBoxAddresses.Items.Add(iff.Name + "\t\t(" +
                    iff.GetIPProperties().UnicastAddresses[0].Address.ToString() + "   /   " + iff.NetworkInterfaceType.ToString());
            }
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InterfaceSelector_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (listBoxAddresses.SelectedIndex < 0)
                listBoxAddresses.SelectedIndex = 0;

            _interface.StartServer(listBoxAddresses.SelectedIndex);
        }
    }
}
