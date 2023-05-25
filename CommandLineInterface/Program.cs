using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using RESTcontrollers;
using System;
using System.Text;
using System.Text.Json;

namespace CommandLineInterface
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Program started");

            Menu mainmenu = new Menu(new string[]{
                "Add a new patient",
                "Add a new examination",
                "Get information about an examination",
                "Trigger EventHub event",
                "Quit"
            });
            bool ShouldExit = false;
            while (!ShouldExit)
            {
                switch (mainmenu.ReadMenu())
                {
                    case 1:
                        //NEW PATIENT
                        Menu.ReadNewPatient();
                        break;
                    case 2:
                        //NEW EXAMINATION
                        Menu.ReadNewExamination();
                        break;
                    case 3:
                        //GET INFORMATION ABPUT AN EXAMINATION
                        ReadData();
                        break;
                    case 4:
                        //TRIGGER EVENTHUB EVENT
                        HttpServiceClass serviceClass = new HttpServiceClass(new HttpClient());
                        serviceClass.SendRequest(HttpMethod.Post, "https://localhost:7252/Add", new Examination());
                        break;
                    case 5:
                        ShouldExit = true;
                        //QUIT
                        break;
                }
            }
        }

        /*
        public static void WriteMenu()
        {
            Console.WriteLine("Please select an action!");
            Console.WriteLine("1-Add a new examination.");
            Console.WriteLine("2-Get information about an examination");
            Console.WriteLine("3-Trigger EventHub event.");
            Console.WriteLine("4-Quit");
        }
        */

        
        public static void ReadData()
        {
            Examination examination = new Examination();
            HttpServiceClass httpService = new HttpServiceClass(new HttpClient());
            //httpService.SendRequest(HttpMethod.Post, "http://localhost:7103/api/Function1", examination);
            if( httpService.SendRequest(HttpMethod.Post, "https://localhost:7252/Add", examination))
            {
                Console.WriteLine("Getting exam is successfull.");
            }
            else
            {
                Console.WriteLine("Getting exam didn't happen.");
            }
        }
        


        //Itt vannak a kommentek:
        //Patient REST rész híányzik
        //Controller, Service, Repository, nagyrészt másolás az Examination-ból
        //ExaminationController kommentek


        //Ez az egész, majd a Repository-ba kerül.
        public async static void EventTrigger()
        {
            int numOfEvents = 1;

            // The Event Hubs client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when events are being published or read regularly.
            // TODO: Replace the <CONNECTION_STRING> and <HUB_NAME> placeholder values
            string ConnectionString = "Endpoint=sb://testforeventhub.servicebus.windows.net/;SharedAccessKeyName=EventHubPolicy;SharedAccessKey=F9ALB5M1I/GvFbjObW7n0kGfNU20JpsGw+AEhKdKPLI=;EntityPath=myeventhubtest";
            string HubName = "myeventhubtest";
            EventHubProducerClient producerClient = new EventHubProducerClient( ConnectionString, HubName);

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
                if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new Examination())))))
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
    }
}