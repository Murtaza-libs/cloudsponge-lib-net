using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CloudSpongeWrapper.ExceptionHandler
{
    public static class ExceptionMapper
    {
        public static Exception GetException(string message)
        {
            throw new Exception(message);
        }
        public static Exception GetException(HttpStatusCode httpStatusCode)
        {
            string message;
            switch (httpStatusCode)
            {
                    
                case HttpStatusCode.BadRequest:
                    message = "Invalid parameters were supplied";
                    break;
                case HttpStatusCode.Unauthorized:
                    message = "Access was denied to the requested resource. Either the supplied domain_key and domain_password did not match a domain or the request attempted to access a resource that you don’t have access to.";
                    break;
                case HttpStatusCode.NotFound:
                    message = "The contacts are not yet available.";
                    break;
                case HttpStatusCode.Gone:
                    message = "The contacts have already been retrieved and have been deleted from CloudSponge.com.";
                    break;
                default:
                    message = "An unkown error occured.";
                    break;
            }

            throw new Exception(message);
        }
    }
}
