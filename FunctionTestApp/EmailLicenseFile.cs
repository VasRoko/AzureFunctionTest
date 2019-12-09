using System;
using System.Text.RegularExpressions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace FunctionTestApp
{
    public static class EmailLicenseFile
    { 
        [FunctionName("EmailLicenseFile")]
        public static void Run([BlobTrigger("licenses/{name}", Connection = "AzureWebJobsStorage")]  string licenseFileContents,
            [SendGrid(ApiKey = "SendGridApiKey")] out SendGridMessage message,
            string name,
            ILogger log)
        {
            var email = Regex.Match(licenseFileContents, @"^OrderEmail\:\ (.+)$", RegexOptions.Multiline).Groups[1].Value;

            log.LogInformation($"Got order from {email}\n License file Name:{name}");
            message = new SendGridMessage();
            message.From = new EmailAddress("localTest@email.com");
            message.AddTo(email);

            var PlainTextBytes = System.Text.Encoding.UTF8.GetBytes(licenseFileContents);
            var base64 = Convert.ToBase64String(PlainTextBytes);

            message.AddAttachment(name, base64, "text/plain");
            message.Subject = "Yor license file";
            message.HtmlContent = "Thank you for your order";
        }
    }
}
