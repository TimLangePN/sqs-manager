using System;
using System.Collections.Generic;

namespace sqshandler
{
    internal class FileWriter
    {
        public static void WriteToJson(string queuename, List<string> messages)
        {
            //writes to desktop i.e. prod-parknow-offstreet-mediumapi-qparkqueue-deadletter20220625T141023.json
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string path = $"{home}/Documents/SQS-messages/";

            bool exists = System.IO.Directory.Exists(path);

            if (!exists)
                System.IO.Directory.CreateDirectory(path);

            System.IO.File.WriteAllLines($"{path}{queuename}{DateTime.Now.ToString("yyyyMMddTHHmmss")}.json", messages);
        }
    }
}
