using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionTestApp.Models
{
    public class ErrorHandler
    {
        public Guid Id { get; private set; }
        public string PartitionKey { get; private set; }
        public string RowKey { get; private set; }
        public string Reciever { get; private set; }
        public string Error { get; private set; }
        public string StackTrace { get; private set; }

        public ErrorHandler(string reciever, string error, string stackTrace)
        {
            Id = new Guid();
            PartitionKey = "errors";
            RowKey = new Guid().ToString();
            Reciever = reciever;
            Error = error;
            StackTrace = stackTrace;
        }
    }
}
