using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Windows;

namespace sqs_handler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public async void Start_Click(object sender, RoutedEventArgs e)
        {
            statuslabel.Text = "Downloading...";

            string role = Handler.GetEnvironment(env.Text);
            string accountid = Handler.GetAccountId(env.Text);


            string accesskey = AwsCredentials.GetCredentials(role, 1);
            string secretkey = AwsCredentials.GetCredentials(role, 2);
            string token = AwsCredentials.GetCredentials(role, 3);

            var credentials = new SessionAWSCredentials(accesskey, secretkey, token);

            List<string> messages = new List<string>();

            AmazonSQSClient sqsClient = new AmazonSQSClient(credentials, Handler.GetRegionEndpoint(region.Text));


            for (int i = 0; i < 10; i++)
            {
                var exc = false;
                try
                {
                    ReceiveMessageResponse response = await SqsMessageHandler.GetMessagesFromQueue(sqsClient, $"https://sqs.{region.Text}.amazonaws.com/{accountid}/{queuename.Text}");

                    foreach (var message in response.Messages)
                    {
                        messages.Add(message.Body);
                    }
                }
                catch (Exception ex)
                {
                    statuslabel.Text = ex.Message;
                    exc = true;
                }
                if (exc == false)
                {
                    FileWriter.WriteToJson(queuename.Text, messages);
                    statuslabel.Text = "Done!";
                }
                else
                {

                }
            }
        }
    }
}