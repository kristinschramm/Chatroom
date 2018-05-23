using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Chatroom
    {
        // member variables
        Queue<Message> ChatRoomQueue;
        public List<Client> ChatRoomClientList;   // how does a new client get added to this
        public string chatRoomName;


        //constructor
        public Chatroom(string chatRoomName)
        {
            this.chatRoomName = chatRoomName;
            //filepath for log 

        }

        //member methods  -- these will be the methods we are writing in server?
        public bool CheckIfUserIsInChatRoomList(string userId)
        {
            foreach (Client client in ChatRoomClientList)
            {
                if (client.UserId == userId)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("You are not currently part of this chat room.");
                  return SendChatRoomJoinRequest();
                }
            }
            return false;
        }

        public bool SendChatRoomJoinRequest()
        {
            Console.WriteLine("Do you want to request to join chat Room?");
            if (UI.GetInput() == "No")
            { return false; }
            else if (UI.GetInput() == "Yes")
            {
                foreach (Client client in ChatRoomClientList)
                {
                    bool response  = false;
                    //send message to each client in list asking if user can join, return bool = repsonse
                    if (response == false)
                    { return response; }
                    else if (response == true)
                    {
                        ChatRoomClientList.Add(client); // this client is the one requesting to be on the list
                    }
                    else { }  //Handle exception
                }
            }
            else
            {
                // handle exception
            }
            return false;
        }

       
        // method to push queue to each member of chatroom
    }
    
}

