using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using static FunctionTestApp.OnPaymentReceived;

namespace FunctionTestApp
{
    public static class GenerateLicense
    {
        [FunctionName("GenerateLicense")]
        public static void Run([QueueTrigger("orders", Connection = "AzureWebJobsStorage")] Order order, 
            [Blob("licenses/{rand-guid}.lic")] TextWriter outputBlob,
            ILogger log)
        {
            outputBlob.WriteLine($"OrderId: {order.OrderId}");
            outputBlob.WriteLine($"OrderEmail: {order.Email}");
            outputBlob.WriteLine($"OrderProductId: {order.ProductId}");
            outputBlob.WriteLine($"OrderPrice: {order.Price}");

            var md5 = System.Security.Cryptography.MD5.Create();
            var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(order.Email = "secret"));
            
            outputBlob.WriteLine($"SecretCode: {BitConverter.ToString(hash).Replace("-", "")}");
        }
    }
}
