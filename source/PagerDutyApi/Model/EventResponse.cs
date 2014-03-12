namespace PagerDutyApi.Model
{
    public class EventResponse
    {        
        /// <summary>
        /// succes if ok
        /// </summary>
        public string status { get; set; }
        public string incident_key { get; set; }
        public string message { get; set; }
    }
}