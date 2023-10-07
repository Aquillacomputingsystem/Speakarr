using System.Collections.Generic;
using NzbDrone.Core.Parser.Model;
using Speakarr.Api.V1.Author;
using Speakarr.Api.V1.Books;
using Speakarr.Http.REST;

namespace Speakarr.Api.V1.Parse
{
    public class ParseResource : RestResource
    {
        public string Title { get; set; }
        public ParsedBookInfo ParsedBookInfo { get; set; }
        public AuthorResource Author { get; set; }
        public List<BookResource> Books { get; set; }
    }
}
