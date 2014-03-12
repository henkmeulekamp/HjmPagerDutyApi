using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HjmPagerDutyApi.Config;
using HjmPagerDutyApi.Model;
using Newtonsoft.Json;

namespace HjmPagerDutyApi
{
    public class IntegrationApi
    {
        private readonly PagerDutyConfig _config;

        public IntegrationApi(PagerDutyConfig config)
        {
            _config = config;
        }


        public IncidentEventResponse Trigger(IncidentEvent newIncident)
        {
            
        }
    }

    public class JsonHttpClient
    {
        public HttpResult Post<TRequest>(string url, TRequest request)
        {
            var jsonData=  JsonConvert.SerializeObject(request, Formatting.Indented);

            // Create the WebRequest
            var webRequest = (HttpWebRequest)WebRequest.Create(url);

            // Create the Headers
            webRequest.ContentType = "application/json";
            webRequest.Accept = "application/json";
            webRequest.Method = "POST";

            // Set the Content Length
            try
            {
                using (var writer = new StreamWriter(webRequest.Get()))
                {
                    writer.Write(postData);
                    writer.Close();
                }
            }
            catch (WebException we)
            {
                // log the error and rethrow the exception so PayPal can process the void
                _logging.Log(LogLevel.Fatal, "PayPalHelpers.SendHttpPost", "Failure during PayPal SEND. Status:" + ExtractHttpException(we), we);
                throw;
            }

            // Get the response
            try
            {
                using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
                using (var stream = webResponse.GetResponseStream())
                {
                    if (stream != null)
                    {
                        var streamReader = new StreamReader(stream);
                        response = streamReader.ReadToEnd();
                    }
                    webResponse.Close();
                }

            }
            catch (WebException we)
            {
                // log the error and rethrow the exception so PayPal can process the void
                _logging.Log(LogLevel.Fatal, "PayPalHelpers.SendHttpPost", " Failure during PayPal RECEIVE. Status=" + ExtractHttpException(we), we);
                throw;
            }

            return response;
        }

        private static string ExtractHttpException(WebException we)
        {
            string message;
            var httpResp = we.Response as HttpWebResponse;
            if (httpResp != null)
            {
                message = string.Format("WebException {0}; {1}{2}{3}",
                    httpResp.StatusCode, httpResp.StatusDescription, Environment.NewLine, we.StackTrace);
            }
            else
            {
                message = string.Format("Exception {0}; {1}{2}{3}",
                                        we.Source, we.Message, Environment.NewLine, we.StackTrace);
            }

            return message;

        }
        

    }

    public class HttpResult
    {
        public HttpStatusCode Statuscode { get; set; }
        public string StatusDescription { get; set; }
        public string Body { get; set; }

        //headers
    }
}
