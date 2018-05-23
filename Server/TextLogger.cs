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

        public string filePath = @"C:\Users\schra\Desktop\DevCodeCamp\C#\Chatroom\Server\ChatLog.txt"; //create text logger with chatroom change name with room name
        public void Log(string message)
        {
            if (!File.Exists(filePath))
            {
                using (StreamWriter streamWriter = File.CreateText(filePath))
                {
                    streamWriter.WriteLine(DateTime.Now + message);
                }
            }
            else
            {
                using (StreamWriter streamWriter = File.AppendText(filePath))
                {
                    streamWriter.WriteLine(DateTime.Now +message);
                }
            }            
        }
    }
}
