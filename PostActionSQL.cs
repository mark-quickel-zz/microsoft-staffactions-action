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
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace Microsoft.StaffActions.Action.Functions
{
    public static class PostActionSQL
    {
        [FunctionName("PostActionSQL")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "mssqlactions")] HttpRequest req,
            [Sql("dbo.StaffActions", ConnectionStringSetting = "SQLDBConnection")] IAsyncCollector<ActionItem> actionItem,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            using (var sr = new StreamReader(req.Body))
            {
                var ai = sr.ReadToEnd();
                var ao = JsonConvert.DeserializeObject<ActionItem>(ai);
                actionItem.AddAsync(ao);
                actionItem.FlushAsync();
            }  

            return new OkObjectResult("OK");  
        }
    }

    public class ActionItem
    {
        public int Id {get;set;}
        public string TrackingNumber {get;set;}
        public string Subject {get;set;}
        public DateTime SuspenseDate {get;set;}
        public bool Locked {get;set;}
        public string ActionResult {get;set;}
        public string Status {get;set;}
        public DateTime Modified {get;set;}
        public DateTime Created {get;set;}
    }

}
