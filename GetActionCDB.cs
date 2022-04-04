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
    public static class GetActionCDB
    {
        // http://localhost:7071/api/actions/A001
        [FunctionName("GetActionCDB")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "actions/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: "actions-db",
                collectionName: "actions",
                ConnectionStringSetting = "CosmosDBConnection",
                SqlQuery = "SELECT TOP 1 * FROM c WHERE c.ID = {id}"
                )] IEnumerable<dynamic> actionItem,
            ILogger log)
        {
            log.LogInformation("Invocation of GetActionCDB");
            return new OkObjectResult(actionItem);
        }


    }

    
}
    

/*
 sample queries 
 Get a list of all actions where there is a child contributor with an id of 1003
 SELECT * FROM c WHERE ARRAY_CONTAINS(c.Contributors, {"ID" : 1003}, true) 
 */