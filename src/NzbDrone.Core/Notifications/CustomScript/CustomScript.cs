using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using FluentValidation.Results;
using NLog;
using NzbDrone.Common.Disk;
using NzbDrone.Common.Extensions;
using NzbDrone.Common.Processes;
using NzbDrone.Common.Serializer;
using NzbDrone.Core.Books;
using NzbDrone.Core.HealthCheck;
using NzbDrone.Core.MediaFiles;
using NzbDrone.Core.ThingiProvider;
using NzbDrone.Core.Validation;

namespace NzbDrone.Core.Notifications.CustomScript
{
    public class CustomScript : NotificationBase<CustomScriptSettings>
    {
        private readonly IDiskProvider _diskProvider;
        private readonly IProcessProvider _processProvider;
        private readonly Logger _logger;

        public CustomScript(IDiskProvider diskProvider, IProcessProvider processProvider, Logger logger)
        {
            _diskProvider = diskProvider;
            _processProvider = processProvider;
            _logger = logger;
        }

        public override string Name => "Custom Script";

        public override string Link => "https://wiki.servarr.com/speakarr/settings#connections";

        public override ProviderMessage Message => new ProviderMessage("Testing will execute the script with the EventType set to Test, ensure your script handles this correctly", ProviderMessageType.Warning);

        public override void OnGrab(GrabMessage message)
        {
            var author = message.Author;
            var remoteBook = message.RemoteBook;
            var releaseGroup = remoteBook.ParsedBookInfo.ReleaseGroup;
            var environmentVariables = new StringDictionary();

            environmentVariables.Add("Speakarr_EventType", "Grab");
            environmentVariables.Add("Speakarr_Author_Id", author.Id.ToString());
            environmentVariables.Add("Speakarr_Author_Name", author.Metadata.Value.Name);
            environmentVariables.Add("Speakarr_Author_GRId", author.Metadata.Value.ForeignAuthorId);
            environmentVariables.Add("Speakarr_Release_BookCount", remoteBook.Books.Count.ToString());
            environmentVariables.Add("Speakarr_Release_BookReleaseDates", string.Join(",", remoteBook.Books.Select(e => e.ReleaseDate)));
            environmentVariables.Add("Speakarr_Release_BookTitles", string.Join("|", remoteBook.Books.Select(e => e.Title)));
            environmentVariables.Add("Speakarr_Release_BookIds", string.Join("|", remoteBook.Books.Select(e => e.Id.ToString())));
            environmentVariables.Add("Speakarr_Release_GRIds", remoteBook.Books.Select(x => x.Editions.Value.Single(e => e.Monitored).ForeignEditionId).ConcatToString("|"));
            environmentVariables.Add("Speakarr_Release_Title", remoteBook.Release.Title);
            environmentVariables.Add("Speakarr_Release_Indexer", remoteBook.Release.Indexer ?? string.Empty);
            environmentVariables.Add("Speakarr_Release_Size", remoteBook.Release.Size.ToString());
            environmentVariables.Add("Speakarr_Release_Quality", remoteBook.ParsedBookInfo.Quality.Quality.Name);
            environmentVariables.Add("Speakarr_Release_QualityVersion", remoteBook.ParsedBookInfo.Quality.Revision.Version.ToString());
            environmentVariables.Add("Speakarr_Release_ReleaseGroup", releaseGroup ?? string.Empty);
            environmentVariables.Add("Speakarr_Download_Client", message.DownloadClientName ?? string.Empty);
            environmentVariables.Add("Speakarr_Download_Client_Type", message.DownloadClientType ?? string.Empty);
            environmentVariables.Add("Speakarr_Download_Id", message.DownloadId ?? string.Empty);

            ExecuteScript(environmentVariables);
        }

        public override void OnReleaseImport(BookDownloadMessage message)
        {
            var author = message.Author;
            var book = message.Book;
            var environmentVariables = new StringDictionary();

            environmentVariables.Add("Speakarr_EventType", "Download");
            environmentVariables.Add("Speakarr_Author_Id", author.Id.ToString());
            environmentVariables.Add("Speakarr_Author_Name", author.Metadata.Value.Name);
            environmentVariables.Add("Speakarr_Author_Path", author.Path);
            environmentVariables.Add("Speakarr_Author_GRId", author.Metadata.Value.ForeignAuthorId);
            environmentVariables.Add("Speakarr_Book_Id", book.Id.ToString());
            environmentVariables.Add("Speakarr_Book_Title", book.Title);
            environmentVariables.Add("Speakarr_Book_GRId", book.Editions.Value.Single(e => e.Monitored).ForeignEditionId.ToString());
            environmentVariables.Add("Speakarr_Book_ReleaseDate", book.ReleaseDate.ToString());
            environmentVariables.Add("Speakarr_Download_Client", message.DownloadClientInfo?.Name ?? string.Empty);
            environmentVariables.Add("Speakarr_Download_Client_Type", message.DownloadClientInfo?.Type ?? string.Empty);
            environmentVariables.Add("Speakarr_Download_Id", message.DownloadId ?? string.Empty);

            if (message.BookFiles.Any())
            {
                environmentVariables.Add("Speakarr_AddedBookPaths", string.Join("|", message.BookFiles.Select(e => e.Path)));
            }

            if (message.OldFiles.Any())
            {
                environmentVariables.Add("Speakarr_DeletedPaths", string.Join("|", message.OldFiles.Select(e => e.Path)));
                environmentVariables.Add("Speakarr_DeletedDateAdded", string.Join("|", message.OldFiles.Select(e => e.DateAdded)));
            }

            ExecuteScript(environmentVariables);
        }

        public override void OnRename(Author author, List<RenamedBookFile> renamedFiles)
        {
            var environmentVariables = new StringDictionary();

            environmentVariables.Add("Speakarr_EventType", "Rename");
            environmentVariables.Add("Speakarr_Author_Id", author.Id.ToString());
            environmentVariables.Add("Speakarr_Author_Name", author.Metadata.Value.Name);
            environmentVariables.Add("Speakarr_Author_Path", author.Path);
            environmentVariables.Add("Speakarr_Author_GRId", author.Metadata.Value.ForeignAuthorId);

            ExecuteScript(environmentVariables);
        }

        public override void OnAuthorAdded(Author author)
        {
            var environmentVariables = new StringDictionary();

            environmentVariables.Add("Speakarr_EventType", "AuthorAdded");
            environmentVariables.Add("Speakarr_Author_Id", author.Id.ToString());
            environmentVariables.Add("Speakarr_Author_Name", author.Metadata.Value.Name);
            environmentVariables.Add("Speakarr_Author_Path", author.Path);
            environmentVariables.Add("Speakarr_Author_GRId", author.Metadata.Value.ForeignAuthorId);

            ExecuteScript(environmentVariables);
        }

        public override void OnAuthorDelete(AuthorDeleteMessage deleteMessage)
        {
            var author = deleteMessage.Author;
            var environmentVariables = new StringDictionary();

            environmentVariables.Add("Speakarr_EventType", "AuthorDelete");
            environmentVariables.Add("Speakarr_Author_Id", author.Id.ToString());
            environmentVariables.Add("Speakarr_Author_Name", author.Name);
            environmentVariables.Add("Speakarr_Author_Path", author.Path);
            environmentVariables.Add("Speakarr_Author_GoodreadsId", author.ForeignAuthorId);
            environmentVariables.Add("Speakarr_Author_DeletedFiles", deleteMessage.DeletedFiles.ToString());

            ExecuteScript(environmentVariables);
        }

        public override void OnBookDelete(BookDeleteMessage deleteMessage)
        {
            var author = deleteMessage.Book.Author.Value;
            var book = deleteMessage.Book;

            var environmentVariables = new StringDictionary();

            environmentVariables.Add("Speakarr_EventType", "BookDelete");
            environmentVariables.Add("Speakarr_Author_Id", author.Id.ToString());
            environmentVariables.Add("Speakarr_Author_Name", author.Name);
            environmentVariables.Add("Speakarr_Author_Path", author.Path);
            environmentVariables.Add("Speakarr_Author_GoodreadsId", author.ForeignAuthorId);
            environmentVariables.Add("Speakarr_Book_Id", book.Id.ToString());
            environmentVariables.Add("Speakarr_Book_Title", book.Title);
            environmentVariables.Add("Speakarr_Book_GoodreadsId", book.ForeignBookId);
            environmentVariables.Add("Speakarr_Book_DeletedFiles", deleteMessage.DeletedFiles.ToString());

            ExecuteScript(environmentVariables);
        }

        public override void OnBookFileDelete(BookFileDeleteMessage deleteMessage)
        {
            var author = deleteMessage.Book.Author.Value;
            var book = deleteMessage.Book;
            var bookFile = deleteMessage.BookFile;
            var edition = bookFile.Edition.Value;

            var environmentVariables = new StringDictionary();

            environmentVariables.Add("Speakarr_EventType", "BookFileDelete");
            environmentVariables.Add("Speakarr_Delete_Reason", deleteMessage.Reason.ToString());
            environmentVariables.Add("Speakarr_Author_Id", author.Id.ToString());
            environmentVariables.Add("Speakarr_Author_Name", author.Name);
            environmentVariables.Add("Speakarr_Author_GoodreadsId", author.ForeignAuthorId);
            environmentVariables.Add("Speakarr_Book_Id", book.Id.ToString());
            environmentVariables.Add("Speakarr_Book_Title", book.Title);
            environmentVariables.Add("Speakarr_Book_GoodreadsId", book.ForeignBookId);
            environmentVariables.Add("Speakarr_BookFile_Id", bookFile.Id.ToString());
            environmentVariables.Add("Speakarr_BookFile_Path", bookFile.Path);
            environmentVariables.Add("Speakarr_BookFile_Quality", bookFile.Quality.Quality.Name);
            environmentVariables.Add("Speakarr_BookFile_QualityVersion", bookFile.Quality.Revision.Version.ToString());
            environmentVariables.Add("Speakarr_BookFile_ReleaseGroup", bookFile.ReleaseGroup ?? string.Empty);
            environmentVariables.Add("Speakarr_BookFile_SceneName", bookFile.SceneName ?? string.Empty);
            environmentVariables.Add("Speakarr_BookFile_Edition_Id", edition.Id.ToString());
            environmentVariables.Add("Speakarr_BookFile_Edition_Name", edition.Title);
            environmentVariables.Add("Speakarr_BookFile_Edition_GoodreadsId", edition.ForeignEditionId);
            environmentVariables.Add("Speakarr_BookFile_Edition_Isbn13", edition.Isbn13);
            environmentVariables.Add("Speakarr_BookFile_Edition_Asin", edition.Asin);

            ExecuteScript(environmentVariables);
        }

        public override void OnBookRetag(BookRetagMessage message)
        {
            var author = message.Author;
            var book = message.Book;
            var bookFile = message.BookFile;
            var environmentVariables = new StringDictionary();

            environmentVariables.Add("Speakarr_EventType", "TrackRetag");
            environmentVariables.Add("Speakarr_Author_Id", author.Id.ToString());
            environmentVariables.Add("Speakarr_Author_Name", author.Metadata.Value.Name);
            environmentVariables.Add("Speakarr_Author_Path", author.Path);
            environmentVariables.Add("Speakarr_Author_GRId", author.Metadata.Value.ForeignAuthorId);
            environmentVariables.Add("Speakarr_Book_Id", book.Id.ToString());
            environmentVariables.Add("Speakarr_Book_Title", book.Title);
            environmentVariables.Add("Speakarr_Book_GRId", book.Editions.Value.Single(e => e.Monitored).ForeignEditionId.ToString());
            environmentVariables.Add("Speakarr_Book_ReleaseDate", book.ReleaseDate.ToString());
            environmentVariables.Add("Speakarr_BookFile_Id", bookFile.Id.ToString());
            environmentVariables.Add("Speakarr_BookFile_Path", bookFile.Path);
            environmentVariables.Add("Speakarr_BookFile_Quality", bookFile.Quality.Quality.Name);
            environmentVariables.Add("Speakarr_BookFile_QualityVersion", bookFile.Quality.Revision.Version.ToString());
            environmentVariables.Add("Speakarr_BookFile_ReleaseGroup", bookFile.ReleaseGroup ?? string.Empty);
            environmentVariables.Add("Speakarr_BookFile_SceneName", bookFile.SceneName ?? string.Empty);
            environmentVariables.Add("Speakarr_Tags_Diff", message.Diff.ToJson());
            environmentVariables.Add("Speakarr_Tags_Scrubbed", message.Scrubbed.ToString());

            ExecuteScript(environmentVariables);
        }

        public override void OnHealthIssue(HealthCheck.HealthCheck healthCheck)
        {
            var environmentVariables = new StringDictionary();

            environmentVariables.Add("Speakarr_EventType", "HealthIssue");
            environmentVariables.Add("Speakarr_Health_Issue_Level", Enum.GetName(typeof(HealthCheckResult), healthCheck.Type));
            environmentVariables.Add("Speakarr_Health_Issue_Message", healthCheck.Message);
            environmentVariables.Add("Speakarr_Health_Issue_Type", healthCheck.Source.Name);
            environmentVariables.Add("Speakarr_Health_Issue_Wiki", healthCheck.WikiUrl.ToString() ?? string.Empty);

            ExecuteScript(environmentVariables);
        }

        public override void OnApplicationUpdate(ApplicationUpdateMessage updateMessage)
        {
            var environmentVariables = new StringDictionary();

            environmentVariables.Add("Speakarr_EventType", "ApplicationUpdate");
            environmentVariables.Add("Speakarr_Update_Message", updateMessage.Message);
            environmentVariables.Add("Speakarr_Update_NewVersion", updateMessage.NewVersion.ToString());
            environmentVariables.Add("Speakarr_Update_PreviousVersion", updateMessage.PreviousVersion.ToString());

            ExecuteScript(environmentVariables);
        }

        public override ValidationResult Test()
        {
            var failures = new List<ValidationFailure>();

            if (!_diskProvider.FileExists(Settings.Path))
            {
                failures.Add(new NzbDroneValidationFailure("Path", "File does not exist"));
            }

            foreach (var systemFolder in SystemFolders.GetSystemFolders())
            {
                if (systemFolder.IsParentPath(Settings.Path))
                {
                    failures.Add(new NzbDroneValidationFailure("Path", $"Must not be a descendant of '{systemFolder}'"));
                }
            }

            if (failures.Empty())
            {
                try
                {
                    var environmentVariables = new StringDictionary();
                    environmentVariables.Add("Speakarr_EventType", "Test");

                    var processOutput = ExecuteScript(environmentVariables);

                    if (processOutput.ExitCode != 0)
                    {
                        failures.Add(new NzbDroneValidationFailure(string.Empty, $"Script exited with code: {processOutput.ExitCode}"));
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    failures.Add(new NzbDroneValidationFailure(string.Empty, ex.Message));
                }
            }

            return new ValidationResult(failures);
        }

        private ProcessOutput ExecuteScript(StringDictionary environmentVariables)
        {
            _logger.Debug("Executing external script: {0}", Settings.Path);

            var processOutput = _processProvider.StartAndCapture(Settings.Path, Settings.Arguments, environmentVariables);

            _logger.Debug("Executed external script: {0} - Status: {1}", Settings.Path, processOutput.ExitCode);
            _logger.Debug($"Script Output: {System.Environment.NewLine}{string.Join(System.Environment.NewLine, processOutput.Lines)}");

            return processOutput;
        }
    }
}
