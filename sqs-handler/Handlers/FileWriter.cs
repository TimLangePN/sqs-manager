using System;
using System.Collections.Generic;

namespace sqs_handler
{
    internal class FileWriter
    {
        public static void WriteToJson(string queuename, List<string> messages)
        {
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            System.IO.File.WriteAllLines($"{home}/Desktop/{queuename}{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", messages);
        }
    }
}
