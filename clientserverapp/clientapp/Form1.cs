using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace clientapp
{

    public partial class Form1 : Form
    {
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public Form1()
        {
            InitializeComponent();
        }
        private void SendLoop(string sRec)
        {
            
                rtxConsola.AppendText("\n Sending Message: " + sRec);
                byte[] buffer = Encoding.ASCII.GetBytes(sRec);
                _clientSocket.Send(buffer);
                byte[] receiveBuf = new byte[1024];
                int rec = _clientSocket.Receive(receiveBuf);
                byte[] data = new byte[rec];
                Array.Copy(receiveBuf, data, rec);
                rtxConsola.AppendText("\n Received: " + Encoding.ASCII.GetString(data));
            
        }

        private void LoopConnect()
        {
            int attempts = 0;
            while (!_clientSocket.Connected)
            {
                try
                {
                    attempts++;
                    _clientSocket.Connect(IPAddress.Loopback, 10000);
                }
                catch (SocketException)
                {

                    rtxConsola.Text = "Connection attempts: " + attempts.ToString();
                }
            }
            rtxConsola.Clear();
            rtxConsola.AppendText("\n Connected at attempt ->" +attempts.ToString());
        }

        private void sendmessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void btSend_Click(object sender, EventArgs e)
        {
            if (!_clientSocket.Connected)
            {
                LoopConnect();
            }
                
            SendLoop(sendmessage.Text);
        }
    }
}
