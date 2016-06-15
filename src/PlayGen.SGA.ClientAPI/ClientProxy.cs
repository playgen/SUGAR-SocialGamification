using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace PlayGen.SGA.ClientAPI
{
    public class ClientProxy
    {
        private string _baseAddress = "https://sga.playgen.com";


        private WebClient CreateWebClient()
        {        
            return new WebClient { BaseAddress = _baseAddress };
        }

        public TResponse Get<TResponse>(string urlSuffix)
        {
            var client = CreateWebClient();

            using (Stream data = client.OpenRead(urlSuffix))
            {
                StreamReader reader = new StreamReader(data);
                var responseString =  reader.ReadToEnd();
                return JsonConvert.DeserializeObject<TResponse>(responseString);
            }
        }


        public TResponse Post<TRequest, TResponse>(string urlSuffix, TRequest payload)
        {

            var payloadString = JsonConvert.SerializeObject(payload);

            using (var client = CreateWebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                var responseString = client.UploadString(urlSuffix, payloadString);
                return JsonConvert.DeserializeObject<TResponse>(responseString);
            }
        }

        public TResponse Put<TRequest, TResponse>(string urlSuffix, TRequest payload)
        {

            var payloadString = JsonConvert.SerializeObject(payload);

            using (var client = CreateWebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                var responseString = client.UploadString(urlSuffix, "PUT", payloadString);
                return JsonConvert.DeserializeObject<TResponse>(responseString);
            }
        }

        public TResponse Delete<TResponse>(string urlSuffix)
        {
            using (var client = CreateWebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                var responseString = client.UploadString(urlSuffix, "DELETE", "");
                return JsonConvert.DeserializeObject<TResponse>(responseString);
            }
        }

    }
}
