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
            Client client = new Client("192.168.0.146", 9999);//this was changed from zip Kristin's IP 8888 specific port



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
                   client.Receive();
               }
            });
              
            Console.ReadLine();
        }
    }
}
