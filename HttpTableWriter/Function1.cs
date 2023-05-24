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
using RESTcontrollers;

namespace HttpTableWriter
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Examination data;
            try
            {
                //Doesn't get exception, should make fields required.
                data = JsonConvert.DeserializeObject<Examination>(requestBody);
            }
            catch (JsonSerializationException exception)
            {
                 return new OkObjectResult(exception.Message);
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
                string rowKey = data.patientId.ToString();
                rowKey = "1";
                TableEntity tableEntity = new TableEntity(partitionKey, rowKey){

                    /*
                { "Eye", "Left" }, { "Dioptry", 5.00 },{ "Cylinder", 21 }, {"Axis", 2 }
                     
                     */
                    {"Eye", data.eye }, {"Dioptry", data.sphereDiopter}, {"Cylinder", data.cylinderDiopter}, {"Axis", data.axis}
                };

                Console.WriteLine($"{tableEntity.RowKey}: {tableEntity["Eye"]} ");
                tableClient.AddEntity(tableEntity);
            }

            return new OkObjectResult(responseMessage);
        }
    }
}
