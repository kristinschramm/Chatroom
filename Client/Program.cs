using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client("192.168.0.127", 8888);//this was changed from zip Kristin's IP 8888 specific port
            Task send = Task.Run(() => { client.Send(); }); //this was changed from zip added TASK was just client.Send
            Task recieve = Task.Run(() => { client.Recieve(); });

            Console.ReadLine();
        }
    }
}
