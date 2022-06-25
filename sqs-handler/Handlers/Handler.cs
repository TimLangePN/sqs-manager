using Amazon;

namespace sqs_handler
{
    internal class Handler
    {
        public static string GetEnvironment(string env)
        {
            if (env == "Phonixx") return "pmgroup-prod";
            else return "parknow-bloxx-prod";
        }
        public static string GetAccountId(string env)
        {
            if (env == "Phonixx") return "660620967782";
            else return "680226270606";
        }

        public static RegionEndpoint GetRegionEndpoint(string region)
        {
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
