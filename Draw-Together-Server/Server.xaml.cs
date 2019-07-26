using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
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
namespace Draw_Together_Server
{
    public partial class MainWindow : Window
    {
        const int PORT = 9999;
        
        BackgroundWorker serverBackgroundWorker;
        TcpListener listener;
        TcpClient server;
        NetworkStream stream;
        public MainWindow()
        {
            InitializeComponent();

            serverBackgroundWorker = new BackgroundWorker();
            serverBackgroundWorker.DoWork += ServerBackgroundWorker_DoWork;
            listener = new TcpListener(IPAddress.Any, PORT);
            listener.Start();
            server = listener.AcceptTcpClient();
            stream = server.GetStream();

        }

        private void ServerBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            byte[] receiveBytes = null;
            var memory = new MemoryStream();
            Dispatcher.Invoke(() =>
                {
                    XAML_Ink.Strokes.Save(memory);
                }
            );
            receiveBytes = memory.ToArray();

            stream.WriteByte( (byte) (receiveBytes.Length / 256 / 256) );
            stream.WriteByte( (byte) (receiveBytes.Length / 256) );
            stream.WriteByte( (byte) (receiveBytes.Length % 256) );

            stream.Write(receiveBytes, 0, receiveBytes.Length);

        }

        private void DrawTypeRadioButton_Click(object sender, RoutedEventArgs e)
        {
            XAML_Ink.EditingMode = (sender as Custom.CustomDrawTypeRadioButton).DrawType;
        }

        private void ColorRadioButton_Click(object sender, RoutedEventArgs e)
        {
            XAML_Ink.DefaultDrawingAttributes.Color = (sender as Custom.CustomColorRadioButton).Color;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (XAML_Ink == null)
            {
                return;
            }
            XAML_Ink.DefaultDrawingAttributes.Width = (sender as Slider).Value * 2;
            XAML_Ink.DefaultDrawingAttributes.Height = (sender as Slider).Value * 2;
        }

        private void XAML_Ink_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            serverBackgroundWorker.RunWorkerAsync();
        }

        private void XAML_Ink_StrokeErased(object sender, RoutedEventArgs e)
        {
            serverBackgroundWorker.RunWorkerAsync();
        }
    }
}
