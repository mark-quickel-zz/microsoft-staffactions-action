using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Microsoft.StaffActions.Action.Functions
{
    public static class PostActionCDB
    {
        [FunctionName("PostActionCDB")]
        public static void Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "actions")] HttpRequest req,
            [CosmosDB(
                databaseName: "actions-db",
                collectionName: "actions",
                ConnectionStringSetting = "CosmosDBConnection"
                )] IAsyncCollector<dynamic> actionItem,
            ILogger log)
        {
            log.LogInformation("Invocation of Post function");

            using (var sr = new StreamReader(req.Body))
            {
                var ai = sr.ReadToEnd();
                actionItem.AddAsync(ai);
            }            
        }
    }
}
