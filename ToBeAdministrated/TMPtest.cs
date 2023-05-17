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

namespace ToBeAdministrated
{
    public static class TMPtest
    {
        [FunctionName("TMPtest")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
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

            try
            {
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
            catch (Exception exception)
            {
                responseMessage = exception.Message;
            }
           
            return new OkObjectResult(responseMessage);
        }
    }
}
