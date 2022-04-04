using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.StaffActions.Action.Functions
{
    public static class GetActionSQL
    {
        [FunctionName("GetActionSQL")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "mssqlactions/{id}")] HttpRequest req,
            [Sql("select * from dbo.StaffActions where TrackingNumber = @Id",
                CommandType = System.Data.CommandType.Text,
                Parameters = "@Id={id}",
                ConnectionStringSetting = "SQLDBConnection")] IEnumerable<dynamic> actionItems,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            return new OkObjectResult(actionItems);
        }
    }

}
