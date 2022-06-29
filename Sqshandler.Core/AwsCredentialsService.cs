using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SQS;

namespace Sqshandler.Core
{
    public class AwsCredentialsService
    {
        public static SessionAWSCredentials GetCredentials(string profile)
        {
            var message = $"Please generate Access/Secret/Session token using Okta AWS CLI";

            if (!string.IsNullOrWhiteSpace(profile))
            {
                if (new CredentialProfileStoreChain().TryGetProfile(profile, out CredentialProfile credProfile))
                {
                    var accessKey = credProfile.Options.AccessKey;
                    var secretKey = credProfile.Options.SecretKey;
                    var sessionToken = credProfile.Options.Token;

                    if (string.IsNullOrWhiteSpace(accessKey) || string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(sessionToken))
                    {
                        throw new Exception(message);
                    }
                    var sessionAWSCredentials = new SessionAWSCredentials(accessKey, secretKey, sessionToken);
                    return sessionAWSCredentials;
                }
            }
            throw new Exception(message);
        }

        public (bool IsError, AmazonSQSClient SqsClient, string ErrorMessage) CreateAwsSqsClient(string env)
        {
            //maps the role + accountid from the selected env, Phonixx/Bloxx 
            SessionAWSCredentials credentials;
            try
            {
                credentials = AwsCredentialsService.GetCredentials(Utils.GetAwsRole(env));
            }
            catch (Exception ex)
            {
                //logging the error message
                return (true, null, $"Error: {ex.Message}");
            }

            //Instantiates the sqsClient
            var client = new AmazonSQSClient(credentials, RegionEndpoint.EUCentral1);
            return (false, client, "");
        }
    }
}