using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;

namespace TMPtableWriter
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";


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
                string rowKey = "1";
                Azure.Data.Tables.TableEntity tableEntity = new Azure.Data.Tables.TableEntity(partitionKey, rowKey){
                { "Product", "Marker Set" }, { "Price", 5.00 },{ "Quantity", 21 } };

                Console.WriteLine($"{tableEntity.RowKey}: {tableEntity["Product"]} costs ${tableEntity.GetDouble("Price")}.");
                tableClient.AddEntity(tableEntity);
            }
            // Make a dictionary entity by defining a <see cref="TableEntity">.
           
          
           

            return new OkObjectResult(responseMessage);
        }
    }
}
