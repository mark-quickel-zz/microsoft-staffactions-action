using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Microsoft.StaffActions.Action.Functions
{
    public static class GetActionsCDB
    {
        // http://localhost:7071/api/actions/
        [FunctionName("GetActionsCDB")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "actions")] HttpRequest req,
            [CosmosDB(
                databaseName: "actions-db",
                collectionName: "actions",
                ConnectionStringSetting = "CosmosDBConnection",
                SqlQuery = "SELECT top 100 * FROM c"
                )] IEnumerable<dynamic> actionItems,
            ILogger log)
        {
            log.LogInformation("Invocation of GetActionsCDB");
            return new OkObjectResult(actionItems);
        }


    }

    
}
    

/*
 sample queries 
 Get a list of all actions where there is a child contributor with an id of 1003
 SELECT * FROM c WHERE ARRAY_CONTAINS(c.Contributors, {"ID" : 1003}, true) 
 */