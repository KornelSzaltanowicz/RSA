using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Windows;
using Helpers;
using RSA;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static TcpClient client;
        private static NetworkStream network;
        private static MessageHelper message;
        private IEnumerable<double> encryptedMessage;
        private EncryptionRsa encryptionRSA;
        private Key publicKey;

        public MainWindow()
        {
            InitializeComponent();
            sendBtn.IsEnabled = false;
        }
        

        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            if (newMessage.Text == null) return;
            encryptedMessage = encryptionRSA.Encrypt(newMessage.Text);
            SendMessage(encryptedMessage);
        }

        private static void SendMessage(IEnumerable<double> msgObject)
        {
            message = SerializeHelper.Serialize(msgObject);
            network.Write(message.Data, 0, message.Data.Length);
            
        }

        private void connectBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client = new TcpClient("localhost", 8080);
                network = client.GetStream();
                connectStatus.Content = "Connected";
                
                publicKey = DeserializeKey().Key;
                sendBtn.IsEnabled = true;
                encryptionRSA = new EncryptionRsa(publicKey);
            }
            catch (Exception)
            {
                connectStatus.Content = "Can't connect to server";
                //throw new NetworkInformationException();
            }
        }

        private static MessageObject DeserializeKey()
        {
            byte[] buffer = new byte[client.ReceiveBufferSize];
            network.Read(buffer, 0, client.ReceiveBufferSize);
            MessageHelper messageHelper = new MessageHelper() { Data = buffer };
            return SerializeHelper.Deserialize(messageHelper) as MessageObject;
        }
    }
}
