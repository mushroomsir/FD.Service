using System;
using System.Net;
using FD.Service.Http;

namespace FD.Service
{
    public class HttpResponseException : Exception
    {
        public HttpResponseMessage Response { get; private set; }
        public HttpResponseException(HttpStatusCode statusCode)
            : this(new HttpResponseMessage(statusCode))
        {

        }
        public HttpResponseException(HttpResponseMessage responseMessage)
        {
            Response = responseMessage;
        }
    }

}
