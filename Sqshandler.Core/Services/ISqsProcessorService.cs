using Amazon;

namespace Sqshandler.Core
{
    public interface ISqsProcessorService
    {
        string GetRole(string env);
        string GetAccount(string env);
        RegionEndpoint GetRegionEndpoint(string region);
    }
}
