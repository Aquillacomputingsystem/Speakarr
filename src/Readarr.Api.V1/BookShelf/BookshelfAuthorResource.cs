using System.Collections.Generic;
using Speakarr.Api.V1.Books;

namespace Speakarr.Api.V1.Bookshelf
{
    public class BookshelfAuthorResource
    {
        public int Id { get; set; }
        public bool? Monitored { get; set; }
        public List<BookResource> Books { get; set; }
    }
}
