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
            Client client = new Client("192.168.0.130", 8888);//this was changed from zip Kristin's IP 8888 specific port

<<<<<<< HEAD

            Parallel.Invoke(() =>
            {
               while (true)
               {
                   client.Send();
               }
            },
            () =>
            {
               while (true)
               {
                   client.Recieve();
               }
            });
              
=======
            Task send = Task.Run(() => { client.Send(); }); //this was changed from zip added TASK was just client.Send
            Task recieve = Task.Run(() => { client.Recieve(); });


            client.Send(); //this was changed from zip added TASK was just client.Send
            client.Recieve(); 

>>>>>>> c9a45154dd4152efdc38702ed4bf4360dd8a55fd
            Console.ReadLine();
        }
    }
}
