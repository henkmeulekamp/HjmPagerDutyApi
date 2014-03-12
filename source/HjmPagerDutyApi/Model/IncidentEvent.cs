using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HjmPagerDutyApi.Model
{
    /// <summary>
    /// http://developer.pagerduty.com/documentation/integration/events/trigger
    /// </summary>
    public class IncidentEvent
    {
        /// <summary>
        /// The GUID of one of your "Generic API" services. This is the "service key" listed on a Generic API's service detail page.
        /// </summary>
        public string service_key { get; set; }
        /// <summary>
        /// Set this to trigger
        /// </summary>
        public string event_type { get; set; }
        /// <summary>
        /// A short description of the problem that led to this trigger. This field (or a truncated version) will be used when generating phone calls, SMS messages and alert emails. It will also appear on the incidents tables in the PagerDuty UI. The maximum length is 1024 characte
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// Identifies the incident to which this trigger event should be applied. If there's no open (i.e. unresolved) incident with this key, a new one will be created. If there's already an open incident with a matching key, this event will be appended to that incident's log. The event key provides an easy way to "de-dup" problem reports. If this field isn't provided, PagerDuty will automatically open a new incident with a unique key.

        /// </summary>
        public string incident_key { get; set; }
        /// <summary>
        /// The name of the monitoring client that is triggering this event.
        /// </summary>
        public string client { get; set; }
        /// <summary>
        /// The URL of the monitoring client that is triggering this event.
        /// </summary>
        public string client_url { get; set; }
        /// <summary>
        /// An arbitrary JSON object containing any data you'd like included in the incident log.
        /// </summary>
        public string details { get; set; }
    }

    public class IncidentEventResponse
    {
        public bool Succes { get; set; }
        /// <summary>
        /// succes if ok
        /// </summary>
        public string status { get; set; }
        public string incident_key { get; set; }
        public string message { get; set; }
    }
}
