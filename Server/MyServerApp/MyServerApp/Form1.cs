using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace MyServerApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static float getFloatOfByte(byte[] bytes, int start = 0)
        {
            return BitConverter.ToSingle(bytes, start);
        }
        static string getStringOfByte(byte[] bytes, int start = 0)
        {
            string output = "";
            for (int i = start; i < bytes.Length; i += 2)
            {
                char vOut = BitConverter.ToChar(bytes, i);
                output += vOut;
            }
            return output;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[1024];
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 11000);
            UdpClient newsock = new UdpClient(ipep);
            Console.WriteLine("Ожидание данных...");
            IPEndPoint mysender = new IPEndPoint(IPAddress.Any, 11000);


            Task.Run(() =>
            {

            while (true)
            {
                data = newsock.Receive(ref mysender);

                if (data[0] == 0)
                {
                    Invoke((MethodInvoker)delegate
                    {
                          richTextBox1.Text += $"GET: {data.Length} bytes" + "\n";
                    });

                    float param1 = getFloatOfByte(data, 1);
                    float param2 = getFloatOfByte(data, 1 + sizeof(float));
                    float param3 = getFloatOfByte(data, 1 + 2 * sizeof(float));
                    string param4 = getStringOfByte(data, 1 + 3 * sizeof(float));


                    Invoke((MethodInvoker) delegate
                    {
                        richTextBox1.Text += "Par1=" + param1.ToString() + "\n";
                        richTextBox1.Text += "Par2=" + param2.ToString() + "\n";
                        richTextBox1.Text += "Par3=" + param3.ToString() + "\n";
                        richTextBox1.Text += "Par4=" + param4.ToString() + "\n";
                    });


                 }
                 else if (data[0] == 1)
                 {
                        Invoke((MethodInvoker) delegate
                        {
                            richTextBox1.Text += $"GET (ERROR): {data.Length} bytes";
                            string error = getStringOfByte(data, 1);
                            richTextBox1.Text += error + "\n";
                        });


                    }
                }

                

             });
            

        }


    }
}
