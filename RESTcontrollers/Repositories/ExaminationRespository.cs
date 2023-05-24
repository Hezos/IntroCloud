using System.Text.Json;
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

        // metódus: csak felszedjük a service-ből meghíváskor, az meg a controller-ből szedi fel meghíváskor
        public bool SendRequest(HttpMethod httpMethod, string url, Examination exam)
        {
            //ServiceClass, ez oldja meg, hogy tudjunk küldeni hívásokat
            HttpServiceClass serviceClass = new HttpServiceClass(new HttpClient());
            //Meghívjuk a hívást, url-re vigyázni!
            return serviceClass.SendRequest(httpMethod, url, exam);
        }

        //Itt majd az olvasásnak kell lennie: valamelyik dokumentációs link
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

        //Egy plusz metódus majd kell ami lekérdezi, mennyi elem szerepel eddig

    }
}
