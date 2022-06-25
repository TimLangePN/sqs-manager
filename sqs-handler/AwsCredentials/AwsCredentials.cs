using System;

namespace sqs_handler
{
    internal class AwsCredentials
    {
        public static string GetCredentials(string profile, int num)
        {
            string test = $"[{profile}]";

            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string[] text = System.IO.File.ReadAllLines($"{home}/.aws/credentials");

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == test)
                {
                    return text[i + num][(text[i + num].IndexOf('=') + 2)..];
                }
            }
            return "Cannot find credentials";
        }
    }
}
