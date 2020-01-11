using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Client_Async
{
    class Program
    {
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static bool enviaraArchivo = false;
        static void Main(string[] args)
        {
            Console.Title = "Server";
            LoopConnect();
            SendLoop();
            Console.ReadLine();
        }

        private static void SendLoop()
        {
            byte[] buffer = null;
            while (true)
            {
                if (!enviaraArchivo)
                {
                    Console.Write("Enter a request: ");
                    string req = Console.ReadLine();
                    if (req.Contains("archivo"))
                    {
                        long length = new System.IO.FileInfo("C:/Users/Rafael/Downloads/SRSMeliusoftFlowysofteweb.pdf").Length;
                        Console.WriteLine("Longitud del archivo: " + length);
                        req += "/" + length.ToString();
                        Console.WriteLine("mensaje a enviar: " + req);
                    }
                    buffer = Encoding.ASCII.GetBytes(req);
                }
                else
                {
                    enviaraArchivo = false;
                }
                _clientSocket.Send(buffer);                

                byte[] receivedBuffer = new byte[1024];
                int rec = _clientSocket.Receive(receivedBuffer);
                byte[] data = new byte[rec];
                Array.Copy(receivedBuffer, data, rec);
                Console.WriteLine("Received: " + Encoding.ASCII.GetString(data));
                string recibido = Encoding.ASCII.GetString(data);
                if (recibido == "envia archivo")
                {
                    var path = "C:/Users/Rafael/Downloads/SRSMeliusoftFlowysofteweb.pdf";
                    long length = new System.IO.FileInfo(path).Length;
                    Console.WriteLine("Longitud del archivo: " + length);
                    buffer = File.ReadAllBytes(path);
                    enviaraArchivo = true;
                }
            }
        }

        private static void LoopConnect()
        {

            int attempts = 0;

            while (!_clientSocket.Connected)
            {
                try
                {
                    attempts++;
                    _clientSocket.Connect(IPAddress.Loopback, 100);
                }
                catch (SocketException)
                {
                    Console.Clear();
                    Console.WriteLine("Connection attempt: " + attempts);
                }
            }

            Console.Clear();
            Console.WriteLine("Connected!");
        }
    }
}
