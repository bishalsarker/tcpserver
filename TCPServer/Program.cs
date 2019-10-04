using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            StartServer();
        }

        public static void StartServer()
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("0.0.0.0");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                char[] bytes = new char[1024];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    StreamReader stream = new StreamReader(client.GetStream());

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = bytes.ToString();
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.

                        //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        string msg = "<doctype html><html><body><h1>Hello world!!</h1></body></html>";

                        StreamWriter writer = new StreamWriter(client.GetStream());
                        // Send back a response.
                        writer.Write("HTTP/1.0 200 OK");
                        writer.Write(Environment.NewLine);
                        writer.Write("Content-Type: text/html; charset=UTF-8");
                        writer.Write(Environment.NewLine);
                        writer.Write("Content-Length: " + msg.Length);
                        writer.Write(Environment.NewLine);
                        writer.Write(Environment.NewLine);
                        writer.Write(msg);
                        writer.Flush();
                        //Console.WriteLine("Sent: {0}", data);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}
