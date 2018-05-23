
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
        Dictionary<int, Client> acceptedClients = new Dictionary<int, Client>();
        Dictionary<string, Client> onlineClients = new Dictionary<string, Client>();

        List<Chatroom> existingChatrooms = new List<Chatroom>();
        //index 0 should be an when used can create new chatroom 
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
                    ReceiveMessage();
                }
            })
            ;

        }


        private void AcceptClient()
        {

            TcpClient clientSocket = default(TcpClient);
            clientSocket = server.AcceptTcpClient();
            Console.WriteLine($"Connected Client");                //this was changed from zip changed from just connected
            NetworkStream stream = clientSocket.GetStream();
            client = new Client(stream, clientSocket, acceptedClients.Count); //this was changed from zip changed client to clients list
            AddNewClient(client);
            client = null;

        }
        private void DisplayMessage()    //chat room needs this 
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
            foreach (Client client in acceptedClients.Values)
            {
                client.Send(message);
            }

        }


        private void AddNewClient(Client client)
        {
            acceptedClients.Add((acceptedClients.Count + 1), client);
            Message message = new Message(client, $"{client.UserId} Connected");
            messageQueue.Enqueue(message);
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
        private void DisplayChatRoom()
        {
            foreach (Chatroom chatroom in existingChatrooms)
            {
                Console.WriteLine(chatroom.chatRoomName);
            }

        }

        public void ChooseChatRoom()
        {
            bool onList = false;
            string inputChatName;
            Console.WriteLine("Which Chat room would you like to join?");
            Console.WriteLine("If you would like to create a new Chat Room, please enter the name.");
            inputChatName = UI.GetInput();
            foreach (Chatroom chatroom in existingChatrooms)
            {
                do
                {
                    if (chatroom.chatRoomName.Equals(inputChatName))
                    {
                        string userId = client.UserId;
                        onList = chatroom.CheckIfUserIsInChatRoomList(userId);
                    }
                    else
                    {
                        Chatroom newChatRoom = new Chatroom(inputChatName);
                        onList = true;
                    }
                }
                while (onList == false);
                //enter the chat room
            }



        }



    }
}


