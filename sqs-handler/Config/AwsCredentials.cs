using System;

namespace sqs_handler
{
    internal class AwsCredentials
    {
        public string accesskey { get; set; }
        public string secretkey { get; set; }
        public string token { get; set; }

        //Gets the credentials from .aws/credentials file
        //It looks for the location of the inserted profile and grabs the first 3 lines underneath it
        public void GetCredentials(string profile)
        {
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string[] text = System.IO.File.ReadAllLines($"{home}/.aws/credentials");

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == $"[{profile}]")
                {
                    accesskey = text[i + 1][(text[i + 1].IndexOf('=') + 2)..];
                    secretkey = text[i + 2][(text[i + 2].IndexOf('=') + 2)..];
                    token = text[i + 3][(text[i + 3].IndexOf('=') + 2)..];
                }
            }
        }
    }
}
