using System.Collections.Generic;

namespace Speakarr.Api.V1.Books
{
    public class BooksMonitoredResource
    {
        public List<int> BookIds { get; set; }
        public bool Monitored { get; set; }
    }
}
