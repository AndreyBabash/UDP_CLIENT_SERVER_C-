using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyClientApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            UdpClient udpClient = new UdpClient();
            try
            {
                udpClient.Connect("127.0.0.1", 11000);
                
                
                var sendBytes = GetParams(textBox1.Text,textBox2.Text,textBox3.Text,textBox4.Text);
                udpClient.Send(sendBytes, sendBytes.Length);
                await Task.Delay(10);
                richTextBox1.Text = $"SEND: {sendBytes.Length} bytes";  
                
            }
            catch (Exception ex)
            {
                richTextBox1.Text = ex.Message;
                
            }

        }

        static private byte[] GetParams(string p1,string p2,string p3,string ps4)
        {
            try
            {
                byte[] output = new byte[] { 0 };
                float param1 = float.Parse(p1);
                output = BytesAdd(output, getByteOfFloat(param1));
                float param2 = float.Parse(p2);
                output = BytesAdd(output, getByteOfFloat(param2));
                float param3 = float.Parse(p3);
                output = BytesAdd(output, getByteOfFloat(param3));
                output = BytesAdd(output, getByteOfString(ps4));
                return output;
            }
            catch (Exception ex)
            {
                byte[] output = new byte[] { 1 };
                output = BytesAdd(output, getByteOfString(ex.Message));
                return output;
            }
        }
        static byte[] getByteOfFloat(float a)
        {
            return BitConverter.GetBytes(a);
        }
        static byte[] getByteOfString(string a)
        {
            byte[] bytes = new byte[a.Length * 2];
            for (int i = 0; i < a.Length; i++)
            {
                var bytes_temp = BitConverter.GetBytes(a[i]);
                bytes[i * 2] = bytes_temp[0];
                bytes[i * 2 + 1] = bytes_temp[1];
            }
            return bytes;
        }
        static byte[] BytesAdd(byte[] main, byte[] add)
        {
            byte[] output = new byte[main.Length + add.Length];
            for (int i = 0; i < main.Length; i++)
            {
                output[i] = main[i];
            }
            for (int i = 0; i < add.Length; i++)
            {
                output[i + main.Length] = add[i];
            }
            return output;
        }
    }
}

