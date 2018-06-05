
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
        List<Client> acceptedClients = new List<Client>();
        Dictionary<string, Client> clientDictionary = new Dictionary<string, Client>();
        Queue<Message> messageQueue = new Queue<Message>();
        Client client;
        ILogger logger;

        TcpListener server;
        public Server(ILogger logger)
        {
            server = new TcpListener(IPAddress.Parse("192.168.0.146"), 9999);
            server.Start();            
            this.logger = logger;
            logger.Log(DateTime.Now + " Server started");
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
            Console.WriteLine($"Connected Client {acceptedClients.Count + 1}");
            logger.Log(DateTime.Now + $" Connected Client {acceptedClients.Count + 1}");
            NetworkStream stream = clientSocket.GetStream();
            client = new Client(stream, clientSocket, (acceptedClients.Count + 1), messageQueue, logger, acceptedClients);
            AddNewClient(client);
            string clientIP = ((clientSocket.Client.RemoteEndPoint).ToString());
            Console.WriteLine(clientIP);
            clientDictionary.Add(clientIP, client);
            Console.WriteLine(clientDictionary.Count + " " + clientDictionary[clientIP]);
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
            lock (clientDictionary)
            {
                foreach (Client client in acceptedClients)
                {
                    client.Send(message);
                }
            }
        }

        private void AddNewClient(Client client)
        {
            acceptedClients.Add(client);
            Message message = new Message(client, $"{client.UserId} Connected");

            messageQueue.Enqueue(message);

            lock (messageQueue)
            {
                messageQueue.Enqueue(message);
            }            
        }

        private void RemoveClient(Client client)
        {
            acceptedClients.Remove(client);
            Message message = new Message(client, $"{client.UserId} has left the chatroom");
            lock (messageQueue)
            {
                messageQueue.Enqueue(message);
            }
        }
    }
}


