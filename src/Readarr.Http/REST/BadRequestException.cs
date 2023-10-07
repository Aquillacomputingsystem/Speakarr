using System.Net;
using Speakarr.Http.Exceptions;

namespace Speakarr.Http.REST
{
    public class BadRequestException : ApiException
    {
        public BadRequestException(object content = null)
            : base(HttpStatusCode.BadRequest, content)
        {
        }
    }
}
