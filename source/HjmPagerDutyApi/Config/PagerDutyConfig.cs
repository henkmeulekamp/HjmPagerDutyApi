using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HjmPagerDutyApi.Config
{
    public class PagerDutyConfig
    {
        public string ApiUrl { get; set; }
        public string AuthToken { get; set; }

        public string EventApiUrl { get; set; }
        public string EventAuthToken { get; set; }
    }
}
