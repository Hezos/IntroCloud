using System.Text.Json;
using System.Text;
using CommandLineInterface;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using EventData = Azure.Messaging.EventHubs.EventData;

namespace RESTcontrollers.Repositories
{
    public class PatientRepository
    {
        public class ExaminationRespository
        {
            private readonly HttpClient _httpClient;
            public ExaminationRespository(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            // metódus: csak felszedjük a service-ből meghíváskor, az meg a controller-ből szedi fel meghíváskor
            public bool SendRequest(HttpMethod httpMethod, string url, Patient patient)
            {
                //ServiceClass, ez oldja meg, hogy tudjunk küldeni hívásokat
                HttpServiceClass serviceClass = new HttpServiceClass(new HttpClient());
                //Meghívjuk a hívást, url-re vigyázni!
                // bool result = serviceClass.SendRequest(httpMethod, url, exam);
                bool result = true;
                serviceClass.SendRequest(httpMethod, url, patient);
                return result;
            }

            //Itt majd az olvasásnak kell lennie: valamelyik dokumentációs link
            public List<Examination> GetRequest(HttpMethod httpMethod, string url)
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, url);
                HttpResponseMessage httpResponseMessage = _httpClient.Send(httpRequestMessage);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    // return JsonSerializer.Deserialize<List<Examination>>(httpRequestMessage.Content.ReadAsStringAsync().Result);

                    return JsonSerializer.Deserialize<List<Examination>>(httpResponseMessage.Content.ReadAsStringAsync().Result);
                }
                return null;
            }

        }
    }
}
