using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Sqshandler.Core;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Sqshandler
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
            SqsProcessorService sqsQueue = new(env.Text);

            SessionAWSCredentials credentials;
            try
            {
                credentials = AwsCredentialsService.GetCredentials(sqsQueue.Role);
            }
            catch (Exception ex)
            {
                statuslabel.Text = ex.Message;
                return;
            }

            //Instantiates the sqsClient
            AmazonSQSClient sqsClient = new(credentials, SqsProcessorService.GetRegionEndpoint(region.Text));

            List<string> messages = new();

            string qUrl = $"https://sqs.{region.Text}.amazonaws.com/{sqsQueue.AccountId}/{queueInput.Text}";

            //Loops 100 times through the amount of messages polled, to make sure we get all the messages.
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    //Gets messages from sqs in batches of 10
                    ReceiveMessageResponse response = await SqsProcessorService.GetMessagesAsync(sqsClient, qUrl);

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
                FileWriterService.WriteToJson(queueInput.Text, messages);
                statuslabel.Text = "Done!";
            }
            if (purgeYes.IsChecked == true)
            {
                try
                {
                    sqsClient.PurgeQueueAsync(qUrl).Wait();
                    statuslabel.Text = "Messages have been purged!";
                }
                catch (Exception ex) { statuslabel.Text = ex.Message; }
            }
        }
    }
}