﻿using Amazon.Runtime;
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
            //maps the role + accountid from the selected env, Phonixx/Bloxx 
            string role = SqsQueueHandler.GetEnvironment(env.Text);
            string accountid = SqsQueueHandler.GetAccountId(env.Text);

            AwsCredentials awscredentials = new();

            awscredentials.GetCredentials(role);

            var credentials = new SessionAWSCredentials(awscredentials.accesskey, awscredentials.secretkey, awscredentials.token);

            //Instantiates the sqsClient
            AmazonSQSClient sqsClient = new(credentials, SqsQueueHandler.GetRegionEndpoint(region.Text));

            List<string> messages = new();
            //Loops 15 times through the amount of messages polled

            var exc = false;
            string queueurl = $"https://sqs.{region.Text}.amazonaws.com/{accountid}/{queuename.Text}";

            for (int i = 0; i < 100; i++)
            {
                try
                {
                    ReceiveMessageResponse response = await SqsMessageHandler.GetMessagesFromSqsQueue(sqsClient, queueurl);

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
                FileWriter.WriteToJson(queuename.Text, messages);
                statuslabel.Text = "Done!";
            }
            if (purgeYes.IsChecked == true)
            {
                try 
                {
                    statuslabel.Text = SqsMessageHandler.PurgeMessagesFromSqsQueue(sqsClient, queueurl);
                }
                catch (Exception ex) { statuslabel.Text = ex.Message; }
            }
        }
    }
}