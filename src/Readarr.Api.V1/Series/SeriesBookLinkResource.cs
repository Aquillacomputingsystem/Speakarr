using System.Collections.Generic;
using System.Linq;
using NzbDrone.Core.Books;
using Speakarr.Http.REST;

namespace Speakarr.Api.V1.Series
{
    public class SeriesBookLinkResource : RestResource
    {
        public string Position { get; set; }
        public int SeriesPosition { get; set; }
        public int SeriesId { get; set; }
        public int BookId { get; set; }
    }

    public static class SeriesBookLinkResourceMapper
    {
        public static SeriesBookLinkResource ToResource(this SeriesBookLink model)
        {
            return new SeriesBookLinkResource
            {
                Id = model.Id,
                Position = model.Position,
                SeriesPosition = model.SeriesPosition,
                SeriesId = model.SeriesId,
                BookId = model.BookId
            };
        }

        public static List<SeriesBookLinkResource> ToResource(this IEnumerable<SeriesBookLink> models)
        {
            return models?.Select(ToResource).ToList();
        }
    }
}
