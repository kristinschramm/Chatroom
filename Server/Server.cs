
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
        Dictionary<string, Client> onlineClients = new Dictionary<string, Client>();

        List<Chatroom> existingChatrooms = new List<Chatroom>();
        //index 0 should be an when used can create new chatroom 

        //Dictionary<int, Client> acceptedClients = new Dictionary<int, Client>(); //dictionary doesn't work 
        List<Client> acceptedClients = new List<Client>();
        

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
            Thread ReceiveMessageFromClient = new Thread(new ThreadStart(client.Receive));
            ReceiveMessageFromClient.Start();
            client = null;                                                                          

        }
        private void DisplayMessage()    //chat room needs this 
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
            try
            {
                foreach (Client client in acceptedClients)
                {
                    client.Send(message);
                }
            }
            catch (Exception)
            {
                //allows program to continue if client exits
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


