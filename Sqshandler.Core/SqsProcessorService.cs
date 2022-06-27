using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Sqshandler.Core
{
    public class SqsProcessorService
    {
        public string Role { get; set; }
        public string AccountId { get; set; }

        public SqsProcessorService(string env)
        {
            //roles
            if (env == "Phonixx") Role = "pmgroup-prod";
            else Role = "parknow-bloxx-prod";

            //AWS Account id's
            if (env == "Phonixx") AccountId = "660620967782";
            else AccountId = "680226270606";

        }

        public static RegionEndpoint GetRegionEndpoint(string region)
        {
            //AWS RegionEndpoints
            switch (region)
            {
                case "eu-central-1":
                    return RegionEndpoint.EUCentral1;
                case "eu-west-1":
                    return RegionEndpoint.EUWest1;
                case "eu-west-2":
                    return RegionEndpoint.EUWest2;
                case "eu-south-1":
                    return RegionEndpoint.EUSouth1;
                default:
                    return RegionEndpoint.EUNorth1;
            }
        }

        //Receives the messages from queue in batches of 10
        public static async Task<ReceiveMessageResponse> GetMessagesAsync(
            IAmazonSQS sqsClient, string qUrl)
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
