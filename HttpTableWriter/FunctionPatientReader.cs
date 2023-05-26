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
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Linq;

namespace HttpTableWriter
{
    public static class FunctionPatientReader
    {
        [FunctionName("FunctionPatientReader")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage = "Table items was displayed";

            string storageUri = "https://practice9storage.table.core.windows.net/";
            string accountName = "practice9storage";
            string storageAccountKey = "OS91dKOnvEmvfCVtCAi9OqeQSgsyCHa5mgXs7qJ+1RudGw0Bj7B2qVYXqNcD4nVMLiDeOWJ1T2cU+AStHKi7AQ==";
            string tableName = "Patient";

            TableServiceClient serviceClient = new TableServiceClient(new Uri(storageUri), new TableSharedKeyCredential(accountName, storageAccountKey));
            Pageable<TableItem> queryTableResults = serviceClient.Query(filter: $"TableName eq '{tableName}'");

            Console.WriteLine("The following are the names of the tables in the query results:");

            // Iterate the <see cref="Pageable"> in order to access queried tables.

            foreach (TableItem table in queryTableResults)
            {
                Console.WriteLine(table.Name);
                log.LogInformation(table.Name);
            }

            // Construct a new <see cref="TableClient" /> using a <see cref="TableSharedKeyCredential" />.
            var tableClient = new TableClient(new Uri(storageUri), tableName, new TableSharedKeyCredential(accountName,
                storageAccountKey));
            string partitionKey = "1";
            // "1" rowid doesn't seem to change anything.
            tableClient.GetEntity<TableEntity>(partitionKey, "2");
            Pageable<TableEntity> queryResultsFilter = tableClient.Query<TableEntity>(filter: $"PartitionKey eq '{partitionKey}'");

            // Iterate the <see cref="Pageable"> to access all queried entities.
            foreach (TableEntity qEntity in queryResultsFilter)
            {
                Console.WriteLine($"The Diopty of examinations: ");
                Console.WriteLine(qEntity);
            }

            //Give me a list and I will fill it with your data
            Console.WriteLine($"The query returned {queryResultsFilter.Count()} entities.");
            List<string> PatientStrings = new List<string>();
            foreach (TableEntity qEntity in queryResultsFilter)
            {
                //  ExaminationResult.Add($"{qEntity.GetInt32("RowKey").Value},{qEntity.GetString("Eye")},{qEntity.GetDouble("Dioptry")}," +
                //  $"{qEntity.GetDouble("Cylinder")}, {qEntity.GetInt32("Axis")}");                
                string patientId = qEntity.GetString("RowKey");
                string FirstName = qEntity.GetString("FirstName");
                string LastName = qEntity.GetString("LastName");
                string time = qEntity.GetDateTime("BirthDate").ToString();

                PatientStrings.Add($"{patientId};{FirstName};{LastName};{qEntity.GetString("Gender")};{time}");
            }
            List<Patient> patients = new List<Patient>();
            foreach (var item in PatientStrings)
            {
                Console.WriteLine(item);
                patients.Add(Patient.GetPatientFromString(item));
            }
            foreach (var item in patients)
            {
                Console.WriteLine(item);
            }
            string JsonData = JsonSerializer.Serialize(patients);
            return new OkObjectResult(JsonData);
        }
    }
}
