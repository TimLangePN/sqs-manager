using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Sqshandler.Core;
using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Sqshandler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ServiceProvider serviceProvider;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<ISqsProcessorService, SqsProcessorService>();
        }

        public async void Start_Click(object sender, RoutedEventArgs e)
        {
            var exc = false;

            var sqsClient = CreateAwsSqsClient();

            if (sqsClient.IsError)
            {
                statuslabel.Text = sqsClient.ErrorMessage;
                return;
            }

            List<string> messages = new();

            string qUrl = $"https://sqs.{region.Text}.amazonaws.com/{Utils.GetAwsAccount(env.Text)}/{queueInput.Text}";

            var sqsProcessorService = serviceProvider.GetService<ISqsProcessorService>();

            //Loops 100 times through the amount of messages polled, to make sure we get all the messages.
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    //Gets messages from sqs in batches of 10
                    ReceiveMessageResponse response = await sqsProcessorService.GetMessagesAsync(sqsClient.SqsClient, qUrl);

                    statuslabel.Text = $"Downloading index: {i}";

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
                    sqsClient.SqsClient.PurgeQueueAsync(qUrl).Wait();
                    statuslabel.Text = "Messages have been purged!";
                }
                catch (Exception ex)
                {
                    statuslabel.Text = ex.Message;
                }
            }
        }

        private (bool IsError, AmazonSQSClient SqsClient, string ErrorMessage) CreateAwsSqsClient()
        {
            //maps the role + accountid from the selected env, Phonixx/Bloxx 
            SessionAWSCredentials credentials;
            try
            {
                credentials = AwsCredentialsService.GetCredentials(Utils.GetAwsRole(env.Text));
            }
            catch (Exception ex)
            {
                //logging the error message
                return (true, null, $"Error: {ex.Message}");
            }

            //Instantiates the sqsClient
            var client = new AmazonSQSClient(credentials, Utils.GetAwsRegion(region.Text));
            return (false, client, "");
        }

    }
}