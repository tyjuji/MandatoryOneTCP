using MandatoryOneLibrary;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace MandatoryOneTCP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server started");

            TcpListener listener = new TcpListener(IPAddress.Any, 4646);
            listener.Start();

            Console.WriteLine("Server ready");

            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                Task task = new Task(() => { HandleClient(socket); });
                task.Start();
                Task.Run(() =>
                {
                    task.Wait();
                    if (task.IsCompletedSuccessfully)
                    {
                        Console.WriteLine("Client disconnected successfully.");
                    }
                });
            }

        }
        private static void HandleClient(TcpClient socket)
        {
            var manager = new BooksManager();

            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);

            HandleRequest(reader, writer, socket);

            void HandleRequest(StreamReader reader, StreamWriter writer, TcpClient socket)
            {
                Console.WriteLine("Waiting for input from client.");

                string message1 = reader.ReadLine();
                Console.WriteLine("Received request: " + message1);
                string message2 = reader.ReadLine();
                Console.WriteLine("Received content: " + message2);

                if (message1.Trim() == "GetAll")
                {
                    var result = manager.GetAll();
                    string jsonString = JsonSerializer.Serialize(result);
                    writer.WriteLine(jsonString);
                    writer.Flush();
                    Console.WriteLine("Sent all books.");
                }

                else if (message1.Trim() == "Get")
                {
                    var result = manager.GetByISBN(message2);
                    string jsonString = JsonSerializer.Serialize(result);
                    writer.WriteLine(jsonString);
                    writer.Flush();
                    Console.WriteLine("Sent book by id: " + message2);
                }

                else if (message1.Trim() == "Save")
                {
                    Book deserialized = JsonSerializer.Deserialize<Book>(message2);
                    manager.PostBook(deserialized);
                    Console.WriteLine("Saved book.");
                }

                else if (message1.Trim() == "Close")
                {
                    socket.Close();
                    return;
                }
                else
                {
                    Console.WriteLine("Request not recognized");
                    writer.WriteLine("Request not recognized");
                    writer.Flush();
                }

                HandleRequest(reader, writer, socket);
            }
        }
    }
}
