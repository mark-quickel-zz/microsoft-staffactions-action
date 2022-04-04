using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Microsoft.StaffActions.Action.Functions
{
    public static class CosmosUserFeed
    {
        [FunctionName("CosmosUserFeed")]
        public static void Run([CosmosDBTrigger(
            databaseName: "actions-db",
            collectionName: "users",
            ConnectionStringSetting = "CosmosDBConnection",
            CreateLeaseCollectionIfNotExists = true,
            LeaseCollectionName = "leases")] IReadOnlyList<dynamic> input, 
            [CosmosDB(
                databaseName: "actions-db",
                collectionName: "actions",
                ConnectionStringSetting = "CosmosDBConnection"
            )] DocumentClient client,
            ILogger log)
        {
            if (input != null && input.Count > 0)
            {
               var uri = UriFactory.CreateDocumentCollectionUri("actions-db","actions");
               var id = input[0].ID;
               var phone = input[0].Phone;
               var email = input[0].Email;

               var documents = client
               .CreateDocumentQuery(uri, 
                    "SELECT c.Contributors FROM c WHERE ARRAY_CONTAINS(c.Contributors, {\"ID\" : " + id + "}, true)" , 
                    new FeedOptions() { EnableCrossPartitionQuery = true});

                foreach (var d in documents)
                {
                    d.Contributors.Phone = phone;
                    d.Contributors.Email = email;
                    var t = client.ReplaceDocumentAsync(uri,d);
                }

            }

            
        }
    }
}
