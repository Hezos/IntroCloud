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
using RESTcontrollers.Models;
using Azure.Data.Tables.Models;
using Azure.Data.Tables;
using TableEntity = Azure.Data.Tables.TableEntity;

namespace Company.Function
{
    public static class EventHubTrigger1
    {
        [FunctionName("EventHubTrigger1")]
        public static async Task Run([EventHubTrigger("myeventhubtest", Connection = "ConnectionString")] EventData[] events, ILogger log)
        {
            var exceptions = new List<Exception>();

            //Fix Json Serialization
            foreach (EventData eventData in events)
            {
                try
                {
                    // Replace these two lines with your processing logic.
                    log.LogInformation($"Event Hub message: {eventData.EventBody}");
                    await Task.Yield();
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
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
            log.LogInformation("It works");

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
                //Logból kiderül, hogy lefut a catch rész is valamiért.
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
                string rowKey = data.Id.ToString();
                rowKey = "1";
                TableEntity tableEntity = new TableEntity(partitionKey, rowKey){

                    /*
                { "Eye", "Left" }, { "Dioptry", 5.00 },{ "Cylinder", 21 }, {"Axis", 2 }
                     
                     */
                    {"Eye", data.Eye }, {"Dioptry", data.Dioptry}, {"Cylinder", data.Cylinder}, {"Axis", data.Axis}
                };

                Console.WriteLine($"{tableEntity.RowKey}: {tableEntity["Eye"]} ");
                tableClient.AddEntity(tableEntity);
            }

        }
    }
}
