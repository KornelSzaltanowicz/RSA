using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Helpers;
using RSA;

namespace Server
{
    class Server
    {
        private static Key privateKey;
        private static Key publicKey;
        private static DecryptionRSA decryptionRsa;
        private static NetworkStream stream;
        private static MessageObject messageObject;
        private static TcpClient client;
        private static int port = 8080;

        static void Main(string[] args)
        {

            TcpListener listen = new TcpListener(IPAddress.Any, port);
            listen.Start();

            Console.WriteLine($"Listening on port: {port}");
            privateKey = RSAKeyGenerator.PrivateKey;
            publicKey = RSAKeyGenerator.PublicKey;

            decryptionRsa = new DecryptionRSA(privateKey);
            messageObject = new MessageObject(publicKey);

            while (true)
            {
                client = listen.AcceptTcpClient();
                Console.WriteLine("client connected..");
                stream = client.GetStream();
                Console.WriteLine("Sending public Key to clients...");

                SendPublicKey();
                
                var deserializedData = SerializeHelper.Deserialize(ReadDataFromClient()) as List<double>;
                var decryptedMessage = decryptionRsa.Decrypt((IEnumerable<double>) deserializedData);

                if (deserializedData != null)
                {
                    var encodedResult = deserializedData.Aggregate(string.Empty, (current, character) => current + character);
                    var decodedASCIIResult = decryptedMessage.Aggregate(string.Empty, (current, character) => current + " : " + character);
                    var decodedResult = decryptedMessage.Aggregate(string.Empty, (current, character) => current + (char)character);

                    Console.WriteLine($"Encoded message: {encodedResult}" + Environment.NewLine);
                    Console.WriteLine($"Encoded message (ASCII): {decodedASCIIResult}" + Environment.NewLine);
                    Console.WriteLine($"Decoded message: {decodedResult}" + Environment.NewLine);
                }
            }
        }

        private static void SendPublicKey()
        {
            var serializedMessage = SerializeHelper.Serialize(messageObject);
            stream.Write(serializedMessage.Data, 0, serializedMessage.Data.Length);
        }

        private static MessageHelper ReadDataFromClient()
        {
            byte[] buffer = new byte[client.ReceiveBufferSize];
            stream.Read(buffer, 0, client.ReceiveBufferSize);

            return new MessageHelper() { Data = buffer };
        }
    }


}
