using System.Net;
using Speakarr.Http.Exceptions;

namespace Speakarr.Http.REST
{
    public class NotFoundException : ApiException
    {
        public NotFoundException(object content = null)
            : base(HttpStatusCode.NotFound, content)
        {
        }
    }
}
