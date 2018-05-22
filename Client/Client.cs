using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    // add adress book here 
    // clients will need method to add member to this adress book
    class Client
    {
        TcpClient clientSocket;
        NetworkStream stream;
        public string userName;
        public Client(string IP, int port)
        {
            //this.userName=CreateUser();
            clientSocket = new TcpClient();
            clientSocket.Connect(IPAddress.Parse(IP), port);
            stream = clientSocket.GetStream();
            
        }
        public void Send()
        {
            string messageString = UI.GetInput();
            byte[] message = Encoding.ASCII.GetBytes(messageString);
            stream.Write(message, 0, message.Count());
        }
        public void Recieve()
        {
            byte[] recievedMessage = new byte[256];
            stream.Read(recievedMessage, 0, recievedMessage.Length);
            UI.DisplayMessage(Encoding.ASCII.GetString(recievedMessage));
            
        }
        public string CreateUser()
        {
            UI.DisplayMessage("Please enter a user name.");
            return userName = UI.GetInput();
        }
    }
}
