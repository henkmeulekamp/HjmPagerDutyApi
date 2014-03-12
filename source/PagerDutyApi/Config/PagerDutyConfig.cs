namespace PagerDutyApi.Config
{
    public class PagerDutyConfig
    {
        public string ApiUrl { get; set; }
        public string AuthToken { get; set; }

        public string EventApiUrl { get; set; }
        public string EventAuthToken { get; set; }
    }
}
