using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using Moq;
using NUnit.Framework;
using NzbDrone.Common.Crypto;
using NzbDrone.Core.Books;
using NzbDrone.Core.Download.Pending;
using NzbDrone.Core.Parser;
using NzbDrone.Core.Parser.Model;
using NzbDrone.Core.Test.Framework;

namespace NzbDrone.Core.Test.Download.Pending.PendingReleaseServiceTests
{
    [TestFixture]
    public class RemovePendingFixture : CoreTest<PendingReleaseService>
    {
        private List<PendingRelease> _pending;
        private Book _book;

        [SetUp]
        public void Setup()
        {
            _pending = new List<PendingRelease>();

            _book = Builder<Book>.CreateNew()
                                       .Build();

            Mocker.GetMock<IPendingReleaseRepository>()
                 .Setup(s => s.AllByAuthorId(It.IsAny<int>()))
                 .Returns(_pending);

            Mocker.GetMock<IPendingReleaseRepository>()
                  .Setup(s => s.All())
                  .Returns(_pending);

            Mocker.GetMock<IAuthorService>()
                  .Setup(s => s.GetAuthor(It.IsAny<int>()))
                  .Returns(new Author());

            Mocker.GetMock<IAuthorService>()
                  .Setup(s => s.GetAuthors(It.IsAny<IEnumerable<int>>()))
                  .Returns(new List<Author> { new Author() });

            Mocker.GetMock<IParsingService>()
                  .Setup(s => s.GetBooks(It.IsAny<ParsedBookInfo>(), It.IsAny<Author>(), null))
                  .Returns(new List<Book> { _book });
        }

        private void AddPending(int id, string book)
        {
            _pending.Add(new PendingRelease
            {
                Id = id,
                Title = "Author.Title-Book.Title.abc-Speakarr",
                ParsedBookInfo = new ParsedBookInfo { BookTitle = book },
                Release = Builder<ReleaseInfo>.CreateNew().Build()
            });
        }

        [Test]
        public void should_remove_same_release()
        {
            AddPending(id: 1, book: "Book");

            var queueId = HashConverter.GetHashInt31(string.Format("pending-{0}-book{1}", 1, _book.Id));

            Subject.RemovePendingQueueItems(queueId);

            AssertRemoved(1);
        }

        [Test]
        public void should_remove_multiple_releases_release()
        {
            AddPending(id: 1, book: "Book 1");
            AddPending(id: 2, book: "Book 2");
            AddPending(id: 3, book: "Book 3");
            AddPending(id: 4, book: "Book 3");

            var queueId = HashConverter.GetHashInt31(string.Format("pending-{0}-book{1}", 3, _book.Id));

            Subject.RemovePendingQueueItems(queueId);

            AssertRemoved(3, 4);
        }

        [Test]
        public void should_not_remove_diffrent_books()
        {
            AddPending(id: 1, book: "Book 1");
            AddPending(id: 2, book: "Book 1");
            AddPending(id: 3, book: "Book 2");
            AddPending(id: 4, book: "Book 3");

            var queueId = HashConverter.GetHashInt31(string.Format("pending-{0}-book{1}", 1, _book.Id));

            Subject.RemovePendingQueueItems(queueId);

            AssertRemoved(1, 2);
        }

        private void AssertRemoved(params int[] ids)
        {
            Mocker.GetMock<IPendingReleaseRepository>().Verify(c => c.DeleteMany(It.Is<IEnumerable<int>>(s => s.SequenceEqual(ids))));
        }
    }
}
