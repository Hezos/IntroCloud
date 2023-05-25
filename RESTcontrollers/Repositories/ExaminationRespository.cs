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
            bool result = serviceClass.SendRequest(httpMethod, url, exam);
            EventTrigger();
            return result;
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

        public async void EventTrigger()
        {
            int numOfEvents = 1;

            // The Event Hubs client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when events are being published or read regularly.
            // TODO: Replace the <CONNECTION_STRING> and <HUB_NAME> placeholder values
            string ConnectionString = "Endpoint=sb://testforeventhub.servicebus.windows.net/;SharedAccessKeyName=EventHubPolicy;SharedAccessKey=F9ALB5M1I/GvFbjObW7n0kGfNU20JpsGw+AEhKdKPLI=;EntityPath=myeventhubtest";
            string HubName = "myeventhubtest";
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
                    new Examination())))))
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
