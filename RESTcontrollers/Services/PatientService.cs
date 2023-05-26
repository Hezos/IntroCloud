using RESTcontrollers.Repositories;
using System.Text.Json;
using System.Text;
using CommandLineInterface;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using EventData = Azure.Messaging.EventHubs.EventData;

namespace RESTcontrollers.Services
{
    public class PatientService
    {
        private HttpClient _httpClient;
        public PatientService()
        {
            // Patient RST rész: Majdnem az egész csak másolás, de a Controller attributumokat, majd le kell cserélni a megfelelőre,
            // http hívás rész, az másolás, de az url-re majd vigyázni kell, mivel nincs a patient-nek eventhub-ja, ezért a link
            // az azure function link lesz
        }

        //Itt szeretnénk http hívást küldeni
        //Mi kell hozzá: Methodús: Get, Post például, url: string amit meghívunk, vizsgálat itt az üzenet tartalma miatt kell
        public bool SendRequest(HttpMethod httpMethod, string url, Patient patient)
        {
            
            //PatientRepository repository = new PatientRepository(new HttpClient());
            
            //return repository.SendRequest(httpMethod, url, patient);
            return true;
        }


        public List<Examination> GetRequest(HttpMethod httpMethod, string url, string BodyContent)
        {

            ExaminationRespository exam = new ExaminationRespository(new HttpClient());
            return exam.GetRequest(httpMethod, url);
        }
    }
}
