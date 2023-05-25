
using System.Text.Json;
using System.Text;

using RESTcontrollers.Repositories;

namespace RESTcontrollers.Services
{
    public class ExaminationService
    {
        private HttpClient _httpClient;
        public ExaminationService()
        {
            // Patient RST rész: Majdnem az egész csak másolás, de a Controller attributumokat, majd le kell cserélni a megfelelőre,
            // http hívás rész, az másolás, de az url-re majd vigyázni kell, mivel nincs a patient-nek eventhub-ja, ezért a link
            // az azure function link lesz
        }

        //Itt szeretnénk http hívást küldeni
        //Mi kell hozzá: Methodús: Get, Post például, url: string amit meghívunk, vizsgálat itt az üzenet tartalma miatt kell
        public bool SendRequest(HttpMethod httpMethod, string url, Examination exam)
        {
            //Repository, hogy meghívjuk a metódusait
            ExaminationRespository repository = new ExaminationRespository(new HttpClient());
            //Továbbhívunk a repository-ra
            return repository.SendRequest(httpMethod, url, exam);
        }

        public dynamic GetRequest(HttpMethod httpMethod, string url, string BodyContent)
        {
            /*
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, url);
            string content = JsonSerializer.Serialize(BodyContent);
            httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = _httpClient.Send(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<Examination>(httpRequestMessage.Content.ReadAsStringAsync().Result);
            }*/

            ExaminationRespository exam = new ExaminationRespository(new HttpClient());
            return exam.GetRequest(httpMethod, url,BodyContent);
        }
    }
}
