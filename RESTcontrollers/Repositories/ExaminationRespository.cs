using System.Text.Json;
using System.Text;
using CommandLineInterface;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using EventData = Azure.Messaging.EventHubs.EventData;

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
           // bool result = serviceClass.SendRequest(httpMethod, url, exam);
            bool result = true;
            EventTrigger(exam);
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

        public async void EventTrigger(Examination examination)
        {
            int numOfEvents = 1;

            // The Event Hubs client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when events are being published or read regularly.
            // TODO: Replace the <CONNECTION_STRING> and <HUB_NAME> placeholder values
            string ConnectionString = "Endpoint=sb://eventhubadministrated.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ZT/AxbwAPELJeMGkB761S6k4E3l1AdQw9+AEhDlC6AI=";
            string HubName = "eventhubadministrated";
            EventHubProducerClient producerClient = new EventHubProducerClient(ConnectionString, HubName);

            // Create a batch of events 
            using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

            for (int i = 1; i <= numOfEvents; i++)
            {
                if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes($"Event {i}"))))
                {
                    // if it is too large for the batch
                    throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
                }
            }

            for (int i = 1; i <= numOfEvents; i++)
            {
                //Okay jsonstring maybe?????????????
                if (!eventBatch.TryAdd(new Azure.Messaging.EventHubs.EventData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(
                    examination)))))
                {
                    // if it is too large for the batch
                    Console.WriteLine("Examination added.");
                }
            }

            try
            {
                // Use the producer client to send the batch of events to the event hub
                await producerClient.SendAsync(eventBatch);
                Console.WriteLine($"A batch of {numOfEvents} events has been published.");
                //AddExamination();
            }
            finally
            {
                await producerClient.DisposeAsync();
            }
        }

        //Egy plusz metódus majd kell ami lekérdezi, mennyi elem szerepel eddig

    }
}
