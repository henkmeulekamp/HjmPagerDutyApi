﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using PagerDutyApi;
using PagerDutyApi.Config;
using PagerDutyApi.Model;
using ServiceStack.Text;

namespace UnitTests.OagerDutyApi
{
    [TestFixture]
    public class IntegrationApiTests
    {
        private const string ConfigFilePath = @"C:\projects\github\config\pagerduty.config";

        [Test]
        public void ConfigFileExists()
        {
            if (!File.Exists(ConfigFilePath))
            {
                
                Assert.Fail(string.Format("Config file should be present: {0}; Content:{1}",
                    ConfigFilePath, XmlSerializer.SerializeToString(new PagerDutyConfig())));
                
            }
        }

        [Test]
        public void ConfigFileValid()
        {
            var config = GetPagerDutyConfig();

            Assert.IsNotNull(config, "Config file should have valid content");

            Assert.IsNotNullOrEmpty(config.EventApiUrl);
            Assert.IsNotNullOrEmpty(config.EventAuthToken);
        }

        [Test]
        public void SerializeEvent()
        {
            var incident = new IncidentEvent
            {
                client = "UnitTest",
                description = "Event triggered by unittest",
                incident_key = "bdf7c75f-90c8-4870-a1c5-4c9855e58767",
                service_key = "Test",
            };

            var json = JsonSerializer.SerializeToString(incident);

            Trace.WriteLine(json);
            Assert.AreEqual("{\"service_key\":\"Test\",\"event_type\":\"trigger\"," +
                            "\"description\":\"Event triggered by unittest\"," +
                            "\"incident_key\":\"bdf7c75f-90c8-4870-a1c5-4c9855e58767\",\"client\":\"UnitTest\"}",
                            json);
        }


        [Test]
        public void CreateAcknowledgeResolve()
        {
            var config = GetPagerDutyConfig();

            /*var config = new PagerDutyConfig
                {
                    EventAuthToken = "YOUR_EVENT_API_KEY_HERE",
                    EventApiUrl = "https://events.pagerduty.com/generic/2010-04-15/create_event.json"
                };
            */

            var incident = new IncidentEvent
                {
                    client = "UnitTest",
                    description = "Event triggered by unittest",
                    incident_key = Guid.NewGuid().ToString(),
                    service_key = config.EventAuthToken,                   
                };

            var apiClient = new IntegrationApi(config);

            var response = apiClient.Trigger(incident);

            Assert.IsTrue(response.IsOk(), "Incident should be created");

            var acknowledge = new AcknowledgeEvent(incident, "Working on it!");

            var ackResponse = apiClient.Acknowledge(acknowledge);

            Assert.IsTrue(ackResponse.IsOk(), "Incident should be acknowledged");

            var resolveEvent = new ResolveEvent(incident, "Fixed!");

            var resResponse = apiClient.Resolve(resolveEvent);
            
            Assert.IsTrue(resResponse.IsOk(), "Incident should be resolved");            
        }


        [Test]
        public void GetOpenIndicidents()
        {            
            var api = new IntegrationApi(GetPagerDutyConfig());

            var incidents = api.GetOpenIncidents();
            Assert.IsNotNull(incidents);
        }

        [Test]
        public void RaiseIfNotKnown()
        {
            var config = GetPagerDutyConfig();
            var subject = "[IPHost] 'Parkmobile NL Web02 on web02-nl.parkmobile.com' - 'down'";
            var api = new IntegrationApi(config);

            var incidents = api.GetOpenIncidents();

            var incident = incidents.FirstOrDefault(i => i.trigger_summary_data != null
                           && i.trigger_summary_data.IndexOf("IPHOST", StringComparison.OrdinalIgnoreCase) >= 0 );

            if (incident == null)
            {
                //raise new
                var newIncident = new IncidentEvent
                {
                    client = "IPHost",
                    description = subject,
                    incident_key = Guid.NewGuid().ToString(),
                    service_key = config.EventAuthToken,
                };

                var response = api.Trigger(newIncident);

                Assert.IsTrue(response.IsOk(), "Incident should be created");
            }
            else
            {
                //already known, keep on going
            }
        }


        [Test]
        public void CloseIfStillOpen()
        {
            var config = GetPagerDutyConfig();
            var subject = "[IPHost] 'Parkmobile NL Web02 on web02-nl.parkmobile.com' - 'ok'";
            var api = new IntegrationApi(config);

            var incidents = api.GetOpenIncidents();

            var incident = incidents.FirstOrDefault(i => i.trigger_summary_data != null
                           && i.trigger_summary_data.IndexOf("IPHOST", StringComparison.OrdinalIgnoreCase) >= 0);

            if (incident != null)
            {
                var resolveEvent = new ResolveEvent(new IncidentEvent
                {
                    incident_key = incident.incident_key                    
                }, subject);

                var resResponse = api.Resolve(resolveEvent);

                Assert.IsTrue(resResponse.IsOk(), "Incident should be resolved");        

            }
            else
            {
                //none open
            }
        }

        private static PagerDutyConfig GetPagerDutyConfig()
        {
            var config = XmlSerializer.DeserializeFromString<PagerDutyConfig>(
                File.ReadAllText(ConfigFilePath));
            return config;
        }
    }
}
