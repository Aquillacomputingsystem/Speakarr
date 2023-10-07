using Speakarr.Api.V1.Author;
using Speakarr.Api.V1.Books;
using Speakarr.Http.REST;

namespace Speakarr.Api.V1.Search
{
    public class
    SearchResource : RestResource
    {
        public string ForeignId { get; set; }
        public AuthorResource Author { get; set; }
        public BookResource Book { get; set; }
    }
}
