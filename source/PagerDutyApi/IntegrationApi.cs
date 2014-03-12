﻿using System;
using System.Net;
using PagerDutyApi.Config;
using PagerDutyApi.Helpers;
using PagerDutyApi.Model;
using ServiceStack;

namespace PagerDutyApi
{
    public class IntegrationApi
    {
        private readonly PagerDutyConfig _config;

        public IntegrationApi(PagerDutyConfig config)
        {
            _config = config;
        }


        public EventResponse Trigger(IncidentEvent newIncident)
        {
            return PostIncident(newIncident);           
        }


        public EventResponse Acknowledge(AcknowledgeEvent incident)
        {
            return PostIncident(incident);
        }

        public EventResponse Resolve(ResolveEvent incident)
        {
            return PostIncident(incident);
        }



        private EventResponse PostIncident<T>(T incident)
        {
            try
            {
                return HttpHelper.SendJsonToUrl<EventResponse>(_config.EventApiUrl, "POST", incident);
            }
            catch (WebException wsex)
            {
                return new EventResponse
                {
                    status = "exception",
                    message = string.Format("WebException:{0}, {1}\n{2}",
                                            wsex.Status,
                                            wsex.GetResponseBody(),
                                            wsex.StackTrace)
                };
            }
            catch (Exception e)
            {
                return new EventResponse
                {
                    status = "exception",
                    message = string.Format("exception:{0}, {1}\n{2}",
                                            e.Source,
                                            e.Message,
                                            e.StackTrace)
                };
            }
        }

    }
}
