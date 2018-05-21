using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    class TextLogger : ILogger
    {

        public string filePath = @"C:\Chatroom\Server\ChatLog.txt"; //create text logger with chatroom change name with room name
        public void Log(string message)
        {
            if (!File.Exists(filePath))
            {
                using (StreamWriter streamWriter = File.CreateText(filePath))
                {
                    streamWriter.WriteLine(message);
                }
            }
            else
            {
                using (StreamWriter streamWriter = File.AppendText(filePath))
                {
                    streamWriter.WriteLine(message);
                }

            }

            }
        }
    }
}
