
using System.Text.Json;
using System.Text;
using CommandLineInterface;
using RESTcontrollers.Repositories;

namespace RESTcontrollers.Services
{
    public class ExaminationService
    {
        private HttpClient _httpClient;
        public ExaminationService()
        {

        }
        public bool SendRequest(HttpMethod httpMethod, string url, Examination exam)
        {
            ExaminationRespository repository = new ExaminationRespository(new HttpClient());
            return repository.SendRequest(httpMethod, url, exam);
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
