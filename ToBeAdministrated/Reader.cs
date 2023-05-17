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
using Azure;
using Azure.Data.Tables;
using Microsoft.WindowsAzure.Storage;
using System.Collections.Concurrent;
using System.Linq;

namespace ToBeAdministrated
{
    public static class Reader
    {
        [FunctionName("Reader")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            Console.WriteLine(responseMessage);

            // Use the <see cref="TableServiceClient"> to query the service. Passing in OData filter strings is optional.

            string storageUri = "";
            string accountName = "";
            string storageAccountKey = "";
            string tableName = "";

            TableServiceClient serviceClient = new TableServiceClient(new Uri(storageUri), new TableSharedKeyCredential(accountName, storageAccountKey));
            Pageable<TableItem> queryTableResults = serviceClient.Query(filter: $"TableName eq '{tableName}'");

            Console.WriteLine("The following are the names of the tables in the query results:");

            // Iterate the <see cref="Pageable"> in order to access queried tables.

            foreach (TableItem table in queryTableResults)
            {
                Console.WriteLine(table.Name);
            }

            
            return new OkObjectResult(responseMessage);
        }
    }
}
