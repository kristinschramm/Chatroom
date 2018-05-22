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
        Dictionary<string, Client> acceptedClients = new Dictionary<string, Client>();
        Dictionary<string, Client> onlineClients = new Dictionary<string, Client>();

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

            AcceptClient();
           /// Parallel.Invoke(() =>
//              {           
//              while (true)
//                   {
//                       AcceptClient();
//                  }
//},
//                 () =>
//                 {
                    
//                    while (true)
//                    {
//                        DisplayMessage();
//                    }
//                 },
//                 ()=>
//                 {
                    
//                    while (true)
//                    {
//                        ReceiveMessage();
//                    }
//                 })
//                 ;
             
         }

        
        private void AcceptClient()
        {
                
                TcpClient clientSocket = default(TcpClient);
                clientSocket = server.AcceptTcpClient();
                Console.WriteLine($"Connected Client"); //this was changed from zip changed from just connected
                NetworkStream stream = clientSocket.GetStream();
                client = new Client(stream, clientSocket); //this was changed from zip changed client to clients list
                CheckForNewClient(client);
                client = null;
                                                                       
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

        private void Respond(String message) //this was changed from zip added parameter

        {
            foreach (Client client in acceptedClients.Values)
            {
                client.Send(message);
            }

        }


        private void CheckForNewClient(Client client)
        {
            foreach (string key in acceptedClients.Keys)
            {
                if (acceptedClients.Keys.Equals(client.UserId))
                {
                    onlineClients.Add(client.UserId, client);
                }
                else
                {
                    acceptedClients.Add(client.UserId, client);
                }
            }
        }
        private void ReceiveMessage()
        {
            foreach (Client client in acceptedClients.Values)
            {
                string body = client.Recieve();
                Message message = new Message(client, body);
                messageQueue.Enqueue(message);
            }// rewrite when dictionary get saved
        }

    }
}
