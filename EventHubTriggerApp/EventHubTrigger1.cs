using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Azure.Data.Tables.Models;
using Azure.Data.Tables;
using TableEntity = Azure.Data.Tables.TableEntity;
using Azure;
using RESTcontrollers;

namespace Company.Function
{
    public static class EventHubTrigger1
    {
        [FunctionName("EventHubTrigger1")]
        public static async Task Run([EventHubTrigger("eventhubadministrated", Connection = "ConnectionString")] EventData[] events, ILogger log)
        {
            var exceptions = new List<Exception>();

            //Fix Json Serialization
            foreach (EventData eventData in events)
            {
                try
                {
                    // Replace these two lines with your processing logic.
                    log.LogInformation($"Event Hub message: {eventData.EventBody}");
                    //await Task.Yield();
                }
                catch (Exception e)
                {
                    // We need to keep processing the rest of the batch - capture this exception and continue.
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.
                    exceptions.Add(e);
                }
            }
            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
            {
                log.LogInformation("More than 1 exceptions acquired.");
                throw new AggregateException(exceptions);
            }

            if (exceptions.Count == 1)
            {
                log.LogInformation("Has 1 exception");
                throw exceptions.Single();
            }


            Examination data;
            try
            {
                string requestBody = "";
                //Doesn't get exception, should make fields required.
                data = JsonConvert.DeserializeObject<Examination>(requestBody);
            }
            catch (JsonSerializationException exception)
            {
            }

            string responseMessage = "";

            string storageUri = "https://practice9storage.table.core.windows.net/";
            string accountName = "practice9storage";
            string storageAccountKey = "OS91dKOnvEmvfCVtCAi9OqeQSgsyCHa5mgXs7qJ+1RudGw0Bj7B2qVYXqNcD4nVMLiDeOWJ1T2cU+AStHKi7AQ==";
            string tableName = "Table1";
            TableServiceClient serviceClient = new TableServiceClient(new Uri(storageUri), new TableSharedKeyCredential(accountName,
                storageAccountKey));
            TableClient tableClient = new TableClient(new Uri(storageUri), tableName, new TableSharedKeyCredential(accountName,
                storageAccountKey));
            try
            {
                // Create a new table. The TableItem class stores properties of the created table.
                TableItem table = serviceClient.CreateTableIfNotExists(tableName);
                //Logb�l kider�l, hogy lefut a catch r�sz is valami�rt.
                //?????????????????????????
                Console.WriteLine($"The created table's name is {table.Name}.");
                tableClient.Create();
                responseMessage = "Table created";
            }
            catch (Exception exception)
            {
                // responseMessage = exception.Message;

                // Deletes the table made previously.
                // serviceClient.DeleteTable(tableName);
                responseMessage = "Table already exists";
                Console.WriteLine("Table already exist.");

                string partitionKey = "Examination";
                //Check for rowkey correction!
                data = new Examination();
                string rowKey;
                // = data.Id.ToString();
                //Ask repository for Id
                //If it wouldn't work that's because of async

                // "1" rowid doesn't seem to change anything.
                tableClient.GetEntity<TableEntity>(partitionKey, "1");
                Pageable<TableEntity> queryResultsFilter = tableClient.Query<TableEntity>(filter: $"PartitionKey eq '{partitionKey}'");
                Console.WriteLine($"The query returned {queryResultsFilter.Count()} entities.");

                data.patientId = (queryResultsFilter.Count() +1);
                rowKey = data.patientId.ToString();
                //Enum work with only custom clas
                string EyeValue = "";
                switch (data.Eye)
                {
                    case Eye.Right:
                        EyeValue = "Right";
                        break;
                    case Eye.Left:
                        EyeValue = "Left";
                        break;
                }
                //Check if the Examination is valid
                TableEntity tableEntity = new TableEntity(partitionKey, rowKey){
                    // Change to Tuple
                { "Eye", "Left" }, { "Dioptry", 5.00 },{ "Cylinder", 21.00 }, {"Axis", 2 }
                //   {"Eye",  EyeValue }, {"Dioptry", data.SphereDiopter}, {"Cylinder", data.CylinderDiopter}, {"Axis", data.Axis}
                };
                Response response;
                Console.WriteLine($"{tableEntity.RowKey}: {tableEntity["Eye"]} ");
                try {
                    response = await tableClient.AddEntityAsync(tableEntity);
                    log.LogInformation(response.ToString());

                }
                catch (RequestFailedException reqException)
                {
                    log.LogWarning(reqException.Message);
                }
            }

        }
    }
}
