using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Draw_Together_Client
{
    public partial class MainWindow : Window
    {
        BackgroundWorker clientBackgroundWorker;
        TcpClient client;
        public MainWindow()
        {
            InitializeComponent();
            clientBackgroundWorker = new BackgroundWorker();
            clientBackgroundWorker.DoWork += ClientBackgroundWorker_DoWork;
            clientBackgroundWorker.RunWorkerAsync();
        }

        private void ClientBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            client = new TcpClient();
            client.Connect("127.0.0.1",9999);
            NetworkStream stream = client.GetStream();

            while(true)
            {
                int len = 0;
                len += stream.ReadByte() * 256 * 256;
                len += stream.ReadByte() * 256;
                len += stream.ReadByte();
                byte[] receivedBytes = new byte[len];
                stream.Read(receivedBytes, 0, len);
                MemoryStream memory = new MemoryStream(receivedBytes);
                StrokeCollection sc = new StrokeCollection(memory);
                Dispatcher.Invoke(() =>
               {
                   XAML_InkCanvas.Strokes = sc;
               });
                
            }

        }
    }
}
