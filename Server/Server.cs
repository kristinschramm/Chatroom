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

        TcpListener server;
        public Server()
        {
            server = new TcpListener(IPAddress.Parse("192.168.0.146"), 9999);
            server.Start();
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
            },
            () =>
            {
                while (true)
                {
                    GetMessageFromClient();
                }

            })
            ;

        }


        public void AcceptClient()
        {
                
            TcpClient clientSocket = default(TcpClient);
            clientSocket = server.AcceptTcpClient();
            Console.WriteLine($"Connected Client");                //this was changed from zip changed from just connected
            NetworkStream stream = clientSocket.GetStream();
            client = new Client(stream, clientSocket, acceptedClientsList.Count); //this was changed from zip changed client to clients list
            AddNewClient(client);
            
                                                                       
        }
        private void DisplayMessage()
        {
            if (messageQueue.Count > 0)
            {
                Message message = messageQueue.Dequeue();
                string body = message.Body;
                string senderName = message.UserId;
                Respond(senderName + ": " + body);
            }
        }

        private void Respond(String message)

        {
            
            Parallel.Invoke(() =>
            {
            
               if (acceptedClientsList.Count > 0)
               {
                    acceptedClientsList[0].Send(message);
               }
           },
           () =>
           {
               if (acceptedClientsList.Count > 1)
               {
                   acceptedClientsList[1].Send(message);
               }
           },
           () =>
           {
               if (acceptedClientsList.Count > 2)
               {
                   acceptedClientsList[2].Send(message);
               }
           })
           ;


        }


        private void AddNewClient(Client client)
        {
            acceptedClientsList.Add(client);
            Message message = new Message(client, $"{client.UserId} Connected");
            messageQueue.Enqueue(message);
            
        }
        public void AddMessageToQueue(Client client)
        {
            
                string body = client.Recieve();
                Message message = new Message(client, body);
                messageQueue.Enqueue(message);
            
            // rewrite when dictionary get saved
            //write a parallel loop to continuously check for messages from each person
        }
        private void GetMessageFromClient()
        {
            Parallel.Invoke(() =>
            {

                if (acceptedClientsList.Count > 0)
                {
                    AddMessageToQueue(acceptedClientsList[0]);
                }
            },
           () =>
           {
               if (acceptedClientsList.Count > 1)
               {
                   AddMessageToQueue(acceptedClientsList[1]);
               }
           },
           () =>
           {
               if (acceptedClientsList.Count > 2)
               {
                   AddMessageToQueue(acceptedClientsList[2]);
               }
           })
           ;

        }

       
    }
}
