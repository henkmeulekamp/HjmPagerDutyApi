using System;
using System.IO;
using System.Linq;
using PagerDutyApi;
using PagerDutyApi.Config;
using PagerDutyApi.Model;
using ServiceStack.Text;

namespace PdIncidents
{
    internal class PagerDutyHelper
    {
        private readonly PagerDutyConfig _config;

        private readonly IntegrationApi _apiClient;
                

        public PagerDutyHelper(PagerDutyConfig config)
        {
            _config = config;
            _apiClient = new IntegrationApi(config);
        }


        public static PagerDutyConfig GetPagerDutyConfig(string configPath)
        {
            var config = XmlSerializer.DeserializeFromString<PagerDutyConfig>(
                File.ReadAllText(configPath));
            return config;
        }


        public void RaiseNew(string subject, string message)
        {
            var incidents = _apiClient.GetOpenIncidents();

            var incident = incidents.FirstOrDefault(i => i.trigger_summary_data != null
                                                         &&
                                                         i.trigger_summary_data.IndexOf(subject,
                                                             StringComparison.OrdinalIgnoreCase) >= 0);

            if (incident == null)
            {
                //raise new
                var newIncident = new IncidentEvent
                {
                    client = "ParkmobilePagerdutyCli",
                    description = subject,
                    incident_key = Guid.NewGuid().ToString(),
                    service_key = _config.EventAuthToken,
                    details = message
                };

                var response = _apiClient.Trigger(newIncident);
                if (response.IsOk())
                {
                    Console.WriteLine("Issue created, key: {0}",
                        response.incident_key);
                }
                else
                {
                    Console.WriteLine("Failed to create issue: {0} {1}",
                        response.status,
                        response.message);
                }
            }
            else
            {
                Console.WriteLine("Issue exists, key: {0}\nSubject:{1}",
                    incident.incident_key,
                    incident.trigger_summary_data);
            }
        }

        public void Resolve(string subject)
        {
            var incidents = _apiClient.GetOpenIncidents();

            var incident = incidents.FirstOrDefault(i => i.trigger_summary_data != null
                                                         &&
                                                         i.trigger_summary_data.IndexOf(subject,
                                                             StringComparison.OrdinalIgnoreCase) >= 0);

            if (incident != null)
            {

                var resolveEvent = new ResolveEvent(
                                new IncidentEvent
                                {
                                    incident_key = incident.incident_key,
                                    service_key = _config.EventAuthToken
                                }, subject);

                var resResponse = _apiClient.Resolve(resolveEvent);

                if (resResponse.IsOk())
                {
                    Console.WriteLine("Issue has been resolved: {0}", resResponse.incident_key);
                }
                else
                {
                    Console.WriteLine("Failed to resolve issue: {0} {1}",
                        resResponse.status,
                        resResponse.message);
                }
            }
            else
            {
                Console.WriteLine("No open issue found for subject.");
            }
        }


        public void Acknowledge(string subject, string message)
        {
            var incidents = _apiClient.GetOpenIncidents();

            var incident = incidents.FirstOrDefault(i => i.trigger_summary_data != null
                                                         && i.trigger_summary_data.IndexOf(subject,
                                                             StringComparison.OrdinalIgnoreCase) >= 0);

            if (incident != null)
            {

                var ack = new AcknowledgeEvent(
                                new IncidentEvent
                                {
                                    incident_key = incident.incident_key,
                                    service_key = _config.EventAuthToken
                                }, message);

                var resResponse = _apiClient.Acknowledge(ack);

                if (resResponse.IsOk())
                {
                    Console.WriteLine("Issue has been acknowledged: {0}", resResponse.incident_key);
                }
                else
                {
                    Console.WriteLine("Failed to acknowledge issue: {0} {1}",
                        resResponse.status,
                        resResponse.message);
                }
            }
            else
            {
                Console.WriteLine("No open issue found for subject.");
            }
        }


    }
}