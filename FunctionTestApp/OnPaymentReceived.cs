using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FunctionTestApp.Models;
using Newtonsoft.Json;

namespace FunctionTestApp
{
    public static class OnPaymentReceived
    {
        [FunctionName("OnPaymentReceived")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
           // [Queue("orders")] IAsyncCollector<Order> orderQueue,
            [Table("orders")] IAsyncCollector<Order> orderTable,
            [Table("error")] IAsyncCollector<ErrorHandler> errorTable,
            ILogger log)
        {
            log.LogInformation("We have recieved a payment");
            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var order = JsonConvert.DeserializeObject<Order>(requestBody);

            order.PartitionKey = "orders";
            order.RowKey = order.OrderId;

            try
            {
                // await orderQueue.AddAsync(order);
                await orderTable.AddAsync(order);
                // log.LogInformation($"Order {order.OrderId} recieved from {order.Email} for {order.ProductId} ");
            } 
            catch(Exception ex)
            { 
                await errorTable.AddAsync(new ErrorHandler("vasylrokochuk@gmail.com", ex.Message, ex.StackTrace));
            }
            
            return new OkObjectResult($"Hello {order.Email}, thank you for your purchase");
        }
         
        public class Order
        {
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }
            public string OrderId { get; set; }
            public string ProductId { get; set; }
            public string Email { get; set; }
            public decimal Price { get; set; }
        }
    }
}
