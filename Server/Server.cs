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


        TcpListener server;
        public Server(Client client)
        {
            server = new TcpListener(IPAddress.Parse("192.168.0.127"), 8888);
            server.Start();
        }
        public void Run()
        {
            AcceptClient();
            string message1 = clients[0].Recieve();//this was changed from zip


            Respond(message1, clients[0]);//this was changed from zip
            Queue<string> queue = new Queue<string>();
            string message = queue.First();
             string message2 = clients[1].Recieve();//this was changed from zip


            string message3 = clients[2].Recieve();//this was changed from zip



        }
        private void AcceptClient()
        {
            for (int i = 0; i < onlineClients.Count; i++)//this was changed from zip just put in loop
            {
                TcpClient clientSocket = default(TcpClient);
                clientSocket = server.AcceptTcpClient();
                Console.WriteLine($"Connected Client {i}"); //this was changed from zip changed from just connected
                NetworkStream stream = clientSocket.GetStream();
                Client newClient = new Client(stream, clientSocket); //this was changed from zip changed client to clients list
                CheckForNewClient(newClient);
            }

            }
       
        private void Respond(string body, Client client) //this was changed from zip added parameter 
        {
             client.Send(body);
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

    }
}
