using NzbDrone.Core.Indexers;
using Speakarr.Http;

namespace Speakarr.Api.V1.Indexers
{
    [V1ApiController]
    public class IndexerController : ProviderControllerBase<IndexerResource, IndexerBulkResource, IIndexer, IndexerDefinition>
    {
        public static readonly IndexerResourceMapper ResourceMapper = new ();
        public static readonly IndexerBulkResourceMapper BulkResourceMapper = new ();

        public IndexerController(IndexerFactory indexerFactory)
            : base(indexerFactory, "indexer", ResourceMapper, BulkResourceMapper)
        {
        }
    }
}
