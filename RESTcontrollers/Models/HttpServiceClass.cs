using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using RESTcontrollers;

namespace CommandLineInterface
{
    public class HttpServiceClass
    {
        private readonly HttpClient _httpClient;
        public HttpServiceClass(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public bool SendRequest(HttpMethod httpMethod, string url, Examination exam)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, url);
            string content = JsonSerializer.Serialize(exam);
            httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = _httpClient.Send(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public bool SendRequest(HttpMethod httpMethod, string url, Patient patient)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, url);
            string content = JsonSerializer.Serialize(patient);
            httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = _httpClient.Send(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public dynamic GetRequest(HttpMethod httpMethod, string url, string BodyContent)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, url);
            string content = JsonSerializer.Serialize(BodyContent);
            httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = _httpClient.Send(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<List<Examination>>(httpRequestMessage.Content.ReadAsStringAsync().Result);
            }
            return "Couldn't handle request";
        }

    }
}
