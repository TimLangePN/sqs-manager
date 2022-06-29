using Amazon.SQS;
using Amazon.SQS.Model;

namespace Sqshandler.Core
{
    public interface ISqsProcessorService
    {
        Task<ReceiveMessageResponse> GetMessagesAsync(IAmazonSQS sqsClient, string qUrl);
        Task<ListQueuesResponse> GetListSqs(IAmazonSQS sqsClient);
    }

    public class SqsProcessorService : ISqsProcessorService
    {
        public SqsProcessorService()
        {
        }

        //Receives the messages from queue in batches of 10
        public async Task<ListQueuesResponse> GetListSqs(IAmazonSQS sqsClient)
            => await sqsClient.ListQueuesAsync(new ListQueuesRequest() { MaxResults = 1000 });

        //Receives the messages from queue in batches of 10
        public async Task<ReceiveMessageResponse> GetMessagesAsync(IAmazonSQS sqsClient, string qUrl)
        {
            return await sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = qUrl,
                MaxNumberOfMessages = 10,
                VisibilityTimeout = 20
            });
        }

    }
}
