﻿using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using RESTcontrollers.Models;
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
            bool ShouldExit = false;
            while (!ShouldExit)
            {
                WriteMenu();
                switch (Convert.ToInt32(Console.ReadLine()))
                {
                    case 1:
                        //Add logic goes here
                        AddExamination();
                        break;
                    case 2:
                        //View logic goes here
                        break;
                    case 3:
                        EventTrigger();
                        break;
                    case 4:
                        ShouldExit = true;
                        break;
                    default:
                        throw new Exception("Wrong number was pressed.");
                }
            }
        }

        public static void WriteMenu()
        {
            Console.WriteLine("Please select an action!");
            Console.WriteLine("1-Add a new examination.");
            Console.WriteLine("2-Get information about an examination");
            Console.WriteLine("3-Trigger EventHub event.");
            Console.WriteLine("4-Quit");
        }

        public static void AddExamination()
        {
            Examination examination = new Examination();
            examination.Id = 2;
            examination.Eye = "Left";
            examination.Dioptry = 5.00;
            examination.Cylinder = 21;
            examination.Axis = 2;

            HttpServiceClass httpService = new HttpServiceClass(new HttpClient());
            httpService.SendRequest(HttpMethod.Post, "http://localhost:7103/api/Function1", examination);
        }

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

            try
            {
                // Use the producer client to send the batch of events to the event hub
                await producerClient.SendAsync(eventBatch);
                Console.WriteLine($"A batch of {numOfEvents} events has been published.");
            }
            finally
            {
                await producerClient.DisposeAsync();
            }
        }
    }
}