using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Sqshandler.Core
{
    public class SqsProcessorService : ISqsProcessorService
    {
        public string Role { get; set; }
        public string AccountId { get; set; }
        public RegionEndpoint Region { get; set; }

        public SqsProcessorService(string env, string region)
        {
            Role = GetRole(env);
            AccountId = GetAccount(env);
            Region = GetRegionEndpoint(region);

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
        public string GetRole(string env)
        {
            //roles
            if (env == "Phonixx") return "pmgroup-prod";
            else return "parknow-bloxx-prod";

        }
        public string GetAccount(string env)
        {
            //AWS Account id's
            if (env == "Phonixx") return "660620967782";
            else return "680226270606";
        }
        public RegionEndpoint GetRegionEndpoint(string region)
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
    }
}
