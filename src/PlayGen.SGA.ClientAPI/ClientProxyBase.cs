using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace PlayGen.SGA.ClientAPI
{
    public abstract class ClientProxyBase
    {
        private readonly string _baseAddress;

        protected ClientProxyBase(string baseAddress)
        {
            if (!(Uri.IsWellFormedUriString(baseAddress, UriKind.Absolute)))
            {
                throw new Exception("Base address is not an absolute or valid URI");
            }
            _baseAddress = baseAddress;
        }

        protected UriBuilder GetUriBuilder(string apiSuffix)
        {
            var separator = "";
            if (!(_baseAddress.EndsWith("/") || apiSuffix.StartsWith("/")))
            {
                separator = "/";
            }
            return new UriBuilder(_baseAddress + separator + apiSuffix);
        }

        private WebRequest CreateRequest(string uri, string method)
        {
            var request = WebRequest.Create(uri);
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
            if (dataStream == null || response.ContentLength == 0)
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

        protected TResponse Get<TResponse>(string uri)
        {
            var request = CreateRequest(uri, "GET");
            var response = (HttpWebResponse)request.GetResponse();
            TestStatus(response);
            return GetResponse<TResponse>(response);
        }

        protected TResponse Post<TRequest, TResponse>(string url, TRequest payload)
        {
            var response = PostPut(url, payload, "POST");
            return GetResponse<TResponse>(response);
        }

        protected void Post<TRequest>(string url, TRequest payload)
        {
            PostPut(url, payload, "POST");
        }

        protected TResponse Put<TRequest, TResponse>(string url, TRequest payload)
        {
            var response = PostPut(url, payload, "PUT");
            return GetResponse<TResponse>(response);
        }

        protected void Put<TRequest>(string url, TRequest payload)
        {
            PostPut(url, payload, "PUT");
        }

        private HttpWebResponse PostPut<TRequest>(string url, TRequest payload, string method)
        {
            var payloadString = JsonConvert.SerializeObject(payload);
            var payloadBytes = Encoding.UTF8.GetBytes(payloadString);
            var request = CreateRequest(url, method);
            SendData(request, payloadBytes);
            var response = (HttpWebResponse)request.GetResponse();
            TestStatus(response);
            return response;
        }

        protected TResponse Delete<TResponse>(string url)
        {
            var response = DeleteRequest(url);
            return GetResponse<TResponse>(response);
        }

        protected void Delete(string url)
        {
            DeleteRequest(url);
        }

        private HttpWebResponse DeleteRequest(string url)
        {
            var request = CreateRequest(url, "DELETE");
            var response = (HttpWebResponse)request.GetResponse();
            TestStatus(response);
            return response;
        }
    }
}
