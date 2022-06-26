using System;

namespace sqshandler
{
    internal class AwsCredentials
    {
        public string Accesskey { get; set; }
        public string Secretkey { get; set; }
        public string Token { get; set; }

        //Gets the credentials from .aws/credentials file
        //It looks for the location of the inserted profile and grabs the first 3 lines underneath it
        public AwsCredentials(string profile)
        {
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string[] text = System.IO.File.ReadAllLines($"{home}/.aws/credentials");

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == $"[{profile}]")
                {
                    Accesskey = text[i + 1][(text[i + 1].IndexOf('=') + 2)..];
                    Secretkey = text[i + 2][(text[i + 2].IndexOf('=') + 2)..];
                    Token = text[i + 3][(text[i + 3].IndexOf('=') + 2)..];
                }
            }
        }
    }
}
