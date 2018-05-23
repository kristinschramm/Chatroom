using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Client
    {
        NetworkStream stream;
        TcpClient client;
        public string UserId;
        Queue<Message> messageQueue;
        ILogger logger;
        
        public Client(NetworkStream Stream, TcpClient Client, int UserCount, Queue<Message> MessageQueue, ILogger Logger)
        {
            stream = Stream;
            client = Client;
            this.UserId = "ChatUser"+(UserCount);
            this.messageQueue = MessageQueue;
            this.logger = Logger;
        }
        public void Send(string Message)
        {
            byte[] message = Encoding.ASCII.GetBytes(Message);
            stream.Write(message, 0, message.Count());
        }
        public void Receive()
        {
            while (true)
            {
                byte[] recievedMessage = new byte[256];
                stream.Read(recievedMessage, 0, recievedMessage.Length);
                string receivedMessageString = Encoding.ASCII.GetString(recievedMessage);
                Console.WriteLine(DateTime.Now + " : "+ UserId + " : " + receivedMessageString);
                logger.Log(DateTime.Now+ " " + UserId +" "+ receivedMessageString);
                Message message = ConvertInputToMessage(receivedMessageString);
                lock (messageQueue)
                {
                    messageQueue.Enqueue(message);
                }
            }
        }
        public Message ConvertInputToMessage(string messageBody)
        {
            string body = messageBody;
            Message message = new Message(this, body);
            return message;
            
        }
    }
}
