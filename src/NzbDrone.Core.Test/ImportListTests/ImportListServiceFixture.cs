namespace NzbDrone.Core.Test.ImportListTests
{
    /*
    public class ImportListServiceFixture : DbTest<ImportListFactory, ImportListDefinition>
    {
        private List<IImportList> _importLists;

        [SetUp]
        public void Setup()
        {
            _importLists = new List<IImportList>();

            _importLists.Add(Mocker.Resolve<GoodreadsOwnedBooks>());

            Mocker.SetConstant<IEnumerable<IImportList>>(_importLists);
        }

        [Test]
        public void should_remove_missing_import_lists_on_startup()
        {
            var repo = Mocker.Resolve<ImportListRepository>();

            Mocker.SetConstant<IImportListRepository>(repo);

            var existingImportLists = Builder<ImportListDefinition>.CreateNew().BuildNew();
            existingImportLists.ConfigContract = typeof(SpeakarrListsSettings).Name;

            repo.Insert(existingImportLists);

            Subject.Handle(new ApplicationStartedEvent());

            AllStoredModels.Should().NotContain(c => c.Id == existingImportLists.Id);
        }
    }*/
}
