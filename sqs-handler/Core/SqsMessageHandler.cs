using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Threading.Tasks;

namespace sqs_handler
{
    internal class SqsMessageHandler
    {
        //Receives the messages from queue in batches of 10
        public static async Task<ReceiveMessageResponse> GetMessagesFromSqsQueue(
            IAmazonSQS sqsClient, string qUrl)
        {
            return await sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = qUrl,
                MaxNumberOfMessages = 10,
                VisibilityTimeout = 20
            });
        }
        public static void PurgeMessagesFromSqsQueue(
            IAmazonSQS sqsClient, string qUrl)
        {
                sqsClient.PurgeQueueAsync(qUrl).Wait();
        }
    }
}
