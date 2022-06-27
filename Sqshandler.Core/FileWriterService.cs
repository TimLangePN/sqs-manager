namespace Sqshandler.Core
{
    public class FileWriterService
    {
        public static void WriteToJson(string queuename, List<string> messages)
        {
            //writes to desktop i.e. prod-parknow-offstreet-mediumapi-qparkqueue-deadletter20220625T141023.json
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string path = $"{home}/Documents/SQS-messages/";

            bool exists = Directory.Exists(path);

            if (!exists)
                Directory.CreateDirectory(path);

            File.WriteAllLines($"{path}{queuename}{DateTime.Now:yyyyMMddTHHmmss}.json", messages);
        }
    }
}
