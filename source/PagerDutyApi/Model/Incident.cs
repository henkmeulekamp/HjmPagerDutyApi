using System;

namespace PagerDutyApi.Model
{
    public class IncidentFilter
    {
        public string status { get; set; }
    }

    public class Incidents
    {
        public Incident[] incidents { get; set; }
    }

    public class Incident
    {
        public string id { get; set; }
        public string incident_number { get; set; }
        public string status { get; set; }
        public DateTime created_on { get; set; }
        public string html_url { get; set; }
        public string incident_key { get; set; }
        public Service service { get; set; }
        public User assigned_to_user { get; set; }
        public User last_status_change_by { get; set; }
        public DateTime last_status_change_on { get; set; }
        public string trigger_summary_data { get; set; }
        public string trigger_details_html_url { get; set; }
        public int number_of_escalations { get; set; }
    }
    public class Service
    {
        public string id { get; set; }
        public string name { get; set; }
        public string html_url { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string time_zone { get; set; }
        public string color { get; set; }
        public string role { get; set; }
        public string avatar_url { get; set; }
        public string user_url { get; set; }
        public bool invitation_sent { get; set; }
        public bool marketing_opt_out { get; set; }

    }
}