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
        Client client1;
        Client client2;
        Client client3;
        List<Client> clients = new List<Client>();        

        
        TcpListener server;
        public Server()
        {
            server = new TcpListener(IPAddress.Parse("192.168.0.127"), 8888);
            server.Start();
        }
        public void Run()
        {
            clients.Add(client1); //this was changed from zip
            clients.Add(client2);//this was changed from zip
            clients.Add(client3);//this was changed from zip
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
            for (int i = 0; i < clients.Count; i++)//this was changed from zip just put in loop
            {
                TcpClient clientSocket = default(TcpClient);
                clientSocket = server.AcceptTcpClient();
                Console.WriteLine($"Connected Client {i}"); //this was changed from zip changed from just connected
                NetworkStream stream = clientSocket.GetStream();
                clients[i] = new Client(stream, clientSocket); //this was changed from zip changed client to clients list
            }

            }
       
        private void Respond(string body, Client client) //this was changed from zip added parameter

        {
             client.Send(body);
        }
        
    }
}
