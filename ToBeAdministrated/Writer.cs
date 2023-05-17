using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;

namespace ToBeAdministrated
{
    public static class Writer
    {
        [FunctionName("Writer")]
        public static async Task Run([EventHubTrigger("samples-workitems", Connection = "")] EventData[] events, ILogger log)
        {
            var exceptions = new List<Exception>();

            foreach (EventData eventData in events)
            {
                try
                {
                    // Replace these two lines with your processing logic.
                    log.LogInformation($"C# Event Hub trigger function processed a message: {eventData.EventBody}");
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

            // Construct a new "TableServiceClient using a TableSharedKeyCredential.

            string storageUri = "";
            string accountName = "";
            string storageAccountKey = "";
            string tableName = "";
            TableServiceClient serviceClient = new TableServiceClient(new Uri(storageUri), new TableSharedKeyCredential(accountName, 
                storageAccountKey));
            // Create a new table. The TableItem class stores properties of the created table.
            TableItem table = serviceClient.CreateTableIfNotExists(tableName);
            Console.WriteLine($"The created table's name is {table.Name}.");

            TableClient tableClient = new TableClient(new Uri(storageUri), tableName, new TableSharedKeyCredential(accountName,
                storageAccountKey));
            tableClient.Create();

            // Make a dictionary entity by defining a <see cref="TableEntity">.
            string partitionKey = "";
            string rowKey = "";
            TableEntity tableEntity = new TableEntity(partitionKey, rowKey){ { "Product", "Marker Set" }, { "Price", 5.00 }, 
                { "Quantity", 21 } };

            Console.WriteLine($"{tableEntity.RowKey}: {tableEntity["Product"]} costs ${tableEntity.GetDouble("Price")}.");
            tableClient.AddEntity(tableEntity);
        }
    }
}
