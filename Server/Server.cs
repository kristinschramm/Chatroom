﻿using System;
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
      //  Dictionary<string, Client> addressBook = new Dictionary<string, Client>();      //(key = IPAddress, value = Client instance 
        //addressBook.add("IPA" ,Client client1);

        Client client;
        Client client2;
        Client client3;
        Queue<Message> messageQueue = new Queue<Message>();

        List<Client> clients = new List<Client>();



        TcpListener server;
        public Server()
        {
            server = new TcpListener(IPAddress.Parse("192.168.0.130"), 8888);
            server.Start();
        }
        public void Run()
        {
            Parallel.Invoke(
                () =>
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
                ()=>
                {
                    while (true)
                    {
                        ReceiveMessage();
                    }
                })
                ;
            
        }
        private void AcceptClient()
        {
                TcpClient clientSocket = default(TcpClient);
                clientSocket = server.AcceptTcpClient();
                Console.WriteLine($"Connected Client {client.UserId}"); //this was changed from zip changed from just connected
                NetworkStream stream = clientSocket.GetStream();
                client = new Client(stream, clientSocket); //this was changed from zip changed client to clients list
            

        }

        private void DisplayMessage()
        {
            if (messageQueue.Count > 0)
            {
                Message message = messageQueue.Dequeue();
                string body=  message.Body;
                string senderName = message.UserId;
                Respond(senderName + ": " + body);
            }
        }
        private void Respond(String message) //this was changed from zip added parameter

        {
            foreach (Client client in clients)
            {
                client.Send(message);
            }

        }

        private void ReceiveMessage()
        {
            foreach (Client client in clients)
            {
                string body = client.Recieve();
                Message message = new Message(client, body);
                messageQueue.Enqueue(message);
            }// rewrite when dictionary get saved
        }
        

    }
}
