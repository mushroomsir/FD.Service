using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace FD.Service.Http
{
   public class HttpResponseMessage
    {
        public StringContent StringContent { get; set; }
        private HttpStatusCode statusCode;
        public HttpStatusCode StatusCode
        {
            get
            {
                return this.statusCode;
            }
            set
            {
                if (value < (HttpStatusCode)0 || value > (HttpStatusCode)999)
                    throw new ArgumentOutOfRangeException("value");
                this.statusCode = value;
            }
        }
        public HttpResponseMessage()
            : this(HttpStatusCode.OK)
        {

        }   
        public HttpResponseMessage(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
