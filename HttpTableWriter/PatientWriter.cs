using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Data.Tables.Models;
using Azure.Data.Tables;
using Azure;
using RESTcontrollers;
using System.Collections.Generic;
using Azure.Messaging.EventHubs;
using System.Linq;

namespace HttpTableWriter
{
    public static class PatientWriter
    {
        [FunctionName("PatientWriter")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var exceptions = new List<Exception>();

            string responseMessage = "";

            string storageUri = "https://practice9storage.table.core.windows.net/";
            string accountName = "practice9storage";
            string storageAccountKey = "OS91dKOnvEmvfCVtCAi9OqeQSgsyCHa5mgXs7qJ+1RudGw0Bj7B2qVYXqNcD4nVMLiDeOWJ1T2cU+AStHKi7AQ==";
            string tableName = "Patient";
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
                tableClient.CreateIfNotExists();
                responseMessage = "Table created";
            }
            catch (Exception exception)
            {
                // responseMessage = exception.Message;

                // Deletes the table made previously.
                // serviceClient.DeleteTable(tableName);
                responseMessage = "Table already exists";
                Console.WriteLine("Table already exist.");

                string partitionKey = "1";
                //Check for rowkey correction!
                string rowKey = "3";

                // "1" rowid doesn't seem to change anything.
                tableClient.GetEntity<TableEntity>(partitionKey, "2");
                Pageable<TableEntity> queryResultsFilter = tableClient.Query<TableEntity>(filter: $"PartitionKey eq '{partitionKey}'");
                Console.WriteLine($"The query returned {queryResultsFilter.Count()} entities.");

                TableEntity tableEntity = new TableEntity(partitionKey, rowKey){
                { "LastName", "Vecser" }, { "FirstNaem", "Bence" },{ "Gender", "male" }, {"BirthDate", DateTime.Now.ToString() }
                };
                Response response;
                Console.WriteLine($"{tableEntity.RowKey}: {tableEntity["Eye"]} ");
                try
                {
                    response = await tableClient.AddEntityAsync(tableEntity);
                    log.LogInformation(response.ToString());

                }
                catch (RequestFailedException reqException)
                {
                    log.LogWarning(reqException.Message);
                }
            }
            return new OkObjectResult("");
        }
    }
}
