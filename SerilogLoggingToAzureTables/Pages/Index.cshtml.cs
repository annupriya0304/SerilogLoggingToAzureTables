using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace SerilogLoggingToAzureTables.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public static string connstring = "DefaultEndpointsProtocol=https;AccountName=pshelloazure;AccountKey=A7gGTOSM7O15dXOABrI1tsIXIB/BVpvqoVAYpiBVTk4SHLD5jzME24P6OPI3/A4mCf3cOWAmdrWIaALaaZ92OQ==;EndpointSuffix=core.windows.net";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public  void OnGet()
        {
            _logger.LogInformation("Requested from index.cshtml");
             AddMessage();
           
        }

        //-------------------------------------------------
        // Create the queue service client
        //-------------------------------------------------
        public  static void AddMessage()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connstring);
            CloudQueueClient cloudQueueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue cloudQueue = cloudQueueClient.GetQueueReference("testqueue");
            PurchaseOrder purchaseOrder = new PurchaseOrder()
            {

                PurchaseOrderId = Guid.NewGuid(),
                PurchaseOrderNumber = "PO-1",
                purchaseOrderLines = new List<PurchaseOrderLine>()
                {
                    new PurchaseOrderLine()
                    {

                        PurchaseOrderLineId= Guid.NewGuid(),
                        LineNumber= "01"
                    }
                }
            };
            CloudQueueMessage queueMessage = new CloudQueueMessage(JsonConvert.SerializeObject(purchaseOrder));
            cloudQueue.AddMessageAsync(queueMessage);
            Console.WriteLine("Message sent");

        }
    }


    public class PurchaseOrder
    {
        public Guid PurchaseOrderId { get; set; }
        public string PurchaseOrderNumber { get; set; }

        public List<PurchaseOrderLine> purchaseOrderLines { get; set; }

    }

    public class PurchaseOrderLine
    {
        public Guid PurchaseOrderLineId { get; set; }
        public string LineNumber { get; set; }

    }
}
