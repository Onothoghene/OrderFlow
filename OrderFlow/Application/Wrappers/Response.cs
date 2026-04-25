using Hangfire.Storage.Monitoring;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Wrappers
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public Response(T data, bool succeeded, string message = null)
        {
            Succeeded = succeeded;
            Message = message;
            Data = data;
        }

        public Response(string message)
        {
            Succeeded = false;
            Message = message;
        }

        public Response(string message, bool succeeded)
        {
            Succeeded = succeeded;
            Message = message;
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
    }
}
