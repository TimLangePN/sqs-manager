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
            var exc = false;

            //maps the role + accountid from the selected env, Phonixx/Bloxx 
            SqsQueue sqsQueue = new(env.Text);

            //AwsCredentials class for the accesskey, secretkey and token
            AwsCredentials awsCredentials = new(sqsQueue.Role);

            var credentials = new SessionAWSCredentials(awsCredentials.Accesskey, awsCredentials.Secretkey, awsCredentials.Token);

            //Instantiates the sqsClient
            AmazonSQSClient sqsClient = new(credentials, SqsQueue.GetRegionEndpoint(region.Text));

            List<string> messages = new();

            string qUrl = $"https://sqs.{region.Text}.amazonaws.com/{sqsQueue.AccountId}/{queueInput.Text}";

            //Loops 100 times through the amount of messages polled, to make sure we get all the messages.
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    //Gets messages from sqs in batches of 10
                    ReceiveMessageResponse response = await SqsQueue.GetMessagesAsync(sqsClient, qUrl);

                    statuslabel.Text = "Downloading...";

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
            }
            if (exc == false)
            {
                FileWriter.WriteToJson(queueInput.Text, messages);
                statuslabel.Text = "Done!";
            }
            if (purgeYes.IsChecked == true)
            {
                try 
                {
                    sqsClient.PurgeQueueAsync(qUrl).Wait();
                    statuslabel.Text ="Messages have been purged!";
                }
                catch (Exception ex) { statuslabel.Text = ex.Message; }
            }
        }
    }
}