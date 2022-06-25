using Amazon.SQS;
using Amazon.SQS.Model;
using System.Threading.Tasks;

namespace sqs_handler
{
    internal class SqsMessageHandler
    {
        public static async Task<ReceiveMessageResponse> GetMessagesFromQueue(
            IAmazonSQS sqsClient, string qUrl)
        {
            return await sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = qUrl,
                MaxNumberOfMessages = 10,
                VisibilityTimeout = 10
                // (Could also request attributes, set visibility timeout, etc.)
            });
        }
    }
}
