using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagerDutyApi.Model
{
    public static class EventResponseExtensions
    {

        public static bool IsOk(this EventResponse response)
        {
            return (response != null
                && response.status.Equals("success", StringComparison.OrdinalIgnoreCase));
        }
    }
}
