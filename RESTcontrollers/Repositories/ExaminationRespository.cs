﻿using System.Text.Json;
using System.Text;
using CommandLineInterface;

namespace RESTcontrollers.Repositories
{
    public class ExaminationRespository
    {
        private readonly HttpClient _httpClient;
        public ExaminationRespository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public bool SendRequest(HttpMethod httpMethod, string url, Examination exam)
        {
            HttpServiceClass serviceClass = new HttpServiceClass(new HttpClient());
            return serviceClass.SendRequest(httpMethod, url, exam);
        }

        public dynamic GetRequest(HttpMethod httpMethod, string url, string BodyContent)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, url);
            string content = JsonSerializer.Serialize(BodyContent);
            httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = _httpClient.Send(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<Examination>(httpRequestMessage.Content.ReadAsStringAsync().Result);
            }
            return "Couldn't handle request";
        }

    }
}