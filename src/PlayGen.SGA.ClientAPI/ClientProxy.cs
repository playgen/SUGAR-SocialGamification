using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace PlayGen.SGA.ClientAPI
{
    public class ClientProxy
    {
        private string _baseAddress = "https://sga.playgen.com";

        private WebRequest CreateRequest(string urlSuffix, string method)
        {
            var request = WebRequest.Create(_baseAddress + urlSuffix);
            request.Method = method;
            return request;
        }

        private void SendData(WebRequest request, byte[] payload)
        {
            request.ContentLength = payload.Length;
            request.ContentType = "application/json";
            var dataStream = request.GetRequestStream();
            dataStream.Write(payload, 0, payload.Length);
            dataStream.Close();
        }

        private TResponse GetResponse<TResponse>(HttpWebResponse response)
        {
            var dataStream = response.GetResponseStream();
            if (dataStream == null || dataStream.Length == 0)
            {
                throw new Exception("Response was empty :(");
            } 
            StreamReader reader = new StreamReader(dataStream);
            return JsonConvert.DeserializeObject<TResponse>(reader.ReadToEnd());
        }

        private void TestStatus(HttpWebResponse response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("API ERROR, Status Code: " + response.StatusCode + ". Message: " + response.StatusDescription);
            }
        }

        public TResponse Get<TResponse>(string urlSuffix)
        {
            var request = CreateRequest(urlSuffix, "GET");
            var response = (HttpWebResponse)request.GetResponse();
            TestStatus(response);
            return GetResponse<TResponse>(response);
        }

        public TResponse Post<TRequest, TResponse>(string urlSuffix, TRequest payload)
        {
            var response = PostPut(urlSuffix, payload, "POST");
            return GetResponse<TResponse>(response);
        }

        public void Post<TRequest>(string urlSuffix, TRequest payload)
        {
            PostPut(urlSuffix, payload, "POST");
        }

        public TResponse Put<TRequest, TResponse>(string urlSuffix, TRequest payload)
        {
            var response = PostPut(urlSuffix, payload, "PUT");
            return GetResponse<TResponse>(response);
        }

        public void Put<TRequest>(string urlSuffix, TRequest payload)
        {
            PostPut(urlSuffix, payload, "PUT");
        }

        private HttpWebResponse PostPut<TRequest>(string urlSuffix, TRequest payload, string method)
        {
            var payloadString = JsonConvert.SerializeObject(payload);
            var payloadBytes = Encoding.UTF8.GetBytes(payloadString);
            var request = CreateRequest(urlSuffix, method);
            SendData(request, payloadBytes);
            var response = (HttpWebResponse)request.GetResponse();
            TestStatus(response);
            return response;
        }

        public TResponse Delete<TResponse>(string urlSuffix)
        {
            var response = DeleteRequest(urlSuffix);
            return GetResponse<TResponse>(response);
        }

        public void Delete(string urlSuffix)
        {
            DeleteRequest(urlSuffix);
        }

        private HttpWebResponse DeleteRequest(string urlSuffix)
        {
            var request = CreateRequest(urlSuffix, "DELETE");
            var response = (HttpWebResponse)request.GetResponse();
            TestStatus(response);
            return response;
        }
    }
}
