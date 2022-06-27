using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

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
    }
}