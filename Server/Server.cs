using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        //Dictionary<int, Client> acceptedClients = new Dictionary<int, Client>();
        //Dictionary<string, Client> onlineClients = new Dictionary<string, Client>();
        List<Client> acceptedClientsList = new List<Client>();
        
        Queue<Message> messageQueue = new Queue<Message>();
        Client client;
        ILogger logger;

        TcpListener server;
        public Server(ILogger logger)
        {
            server = new TcpListener(IPAddress.Parse("192.168.0.146"), 9999);
            server.Start();
            this.logger = logger;
        }
        public void Run()
        {
            Parallel.Invoke(() =>
            {
                while (true)
                {
                    AcceptClient();
                }
            },
            () =>
            {
                while (true)
                {
                    DisplayMessage();
                }
            })
            ;

        }


        public void AcceptClient()
        {                
            TcpClient clientSocket = default(TcpClient);
            clientSocket = server.AcceptTcpClient();
            Console.WriteLine($"Connected Client {acceptedClientsList.Count + 1}");
            logger.Log($"Connected Client {acceptedClientsList.Count + 1}");
            NetworkStream stream = clientSocket.GetStream();
            client = new Client(stream, clientSocket, acceptedClientsList.Count, messageQueue, logger); 
            AddNewClient(client);
            Thread ReceiveMessageFromClient = new Thread(new ThreadStart(client.Receive));
            ReceiveMessageFromClient.Start();
            client = null;           
                                                                       
        }
        private void DisplayMessage()
        {
            if (messageQueue.Count > 0)
            {
                lock (messageQueue)
                {
                    Message message = messageQueue.Dequeue();
                    string body = message.Body;
                    string senderName = message.UserId;
                    Respond(senderName + ": " + body);
                }
            }
            
        }

        private void Respond(String message)

        {           
            foreach (Client client in acceptedClientsList)
            {
                client.Send(message);
            }

        }

        private void AddNewClient(Client client)
        {
            acceptedClientsList.Add(client);
            Message message = new Message(client, $"{client.UserId} Connected");
            lock (messageQueue)
            {
                messageQueue.Enqueue(message);
            }            
        }
        
        private void RemoveClient(Client client)
        {
            acceptedClientsList.Remove(client);
            Message message = new Message(client, $"{client.UserId} has left the chatroom");
            lock (messageQueue)
            {
                messageQueue.Enqueue(message);
            }
        }

       
    }
}
