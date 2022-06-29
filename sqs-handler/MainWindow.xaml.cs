using Amazon.SQS;
using Amazon.SQS.Model;
using Sqshandler.Core;
using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

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
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
            InitializeComponent();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<ISqsProcessorService, SqsProcessorService>();
            services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
        }

        public async void Start_Click(object sender, RoutedEventArgs e)
        {
            var exc = false;

            AwsCredentialsService awsCredentials = new();


            var sqsClient = awsCredentials.CreateAwsSqsClient(env.Text);

            if (sqsClient.IsError)
            {
                statuslabel.Text = sqsClient.ErrorMessage;
                return;
            }

            List<string> messages = new();

            string qUrl = $"https://sqs.{region.Text}.amazonaws.com/{Utils.GetAwsAccount(env.Text)}/{ddlQueue.ItemStringFormat}";

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
                FileWriterService.WriteToJson(ddlQueue.ItemStringFormat, messages);
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

        private async void Window_Init(object sender, EventArgs e)
        {
            SqsProcessorService sqsProcessor = new();

            AwsCredentialsService awsCredentials = new();

            var phonixxSqsClient = awsCredentials.CreateAwsSqsClient("Phonixx").SqsClient;
            var bloxxSqsClient = awsCredentials.CreateAwsSqsClient("Bloxx").SqsClient;

            try
            {
                ListQueuesResponse respPhonixx = await sqsProcessor.GetListSqs(phonixxSqsClient);
                ListQueuesResponse respBloxx = await sqsProcessor.GetListSqs(bloxxSqsClient);

                IEnumerable<string> allddlQueues = respPhonixx.QueueUrls.Union(respPhonixx.QueueUrls);

                foreach (string queue in allddlQueues)
                {
                    if (queue.EndsWith("-deadletter")) ddlQueue.Items.Add(queue);
                }
            }
            catch (Exception exc)
            {
                statuslabel.Text = exc.Message;
                return;
            }
        }
    }
}