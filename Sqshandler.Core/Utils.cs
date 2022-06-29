using Amazon;

namespace Sqshandler.Core
{
    public static class Utils
    {
        internal const string Const_Phonixx = "Phonixx";

        public static string GetAwsRole(string env)
        {
            return env.Equals(Const_Phonixx, StringComparison.InvariantCultureIgnoreCase)
                ? "pmgroup-prod"
                : "parknow-bloxx-prod";
        }

        public static RegionEndpoint GetAwsRegion(string region)
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

        //AWS Account id's
        public static string GetAwsAccount(string env) => env.Equals(Const_Phonixx, StringComparison.InvariantCultureIgnoreCase) ? "660620967782" : "680226270606";
    }
}
