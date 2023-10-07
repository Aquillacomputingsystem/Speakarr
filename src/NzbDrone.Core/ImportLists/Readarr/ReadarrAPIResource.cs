using System.Collections.Generic;

namespace NzbDrone.Core.ImportLists.Speakarr
{
    public class SpeakarrAuthor
    {
        public string AuthorName { get; set; }
        public int Id { get; set; }
        public string ForeignAuthorId { get; set; }
        public string Overview { get; set; }
        public List<MediaCover.MediaCover> Images { get; set; }
        public bool Monitored { get; set; }
        public int QualityProfileId { get; set; }
        public string RootFolderPath { get; set; }
        public HashSet<int> Tags { get; set; }
    }

    public class SpeakarrEdition
    {
        public string Title { get; set; }
        public string ForeignEditionId { get; set; }
        public string Overview { get; set; }
        public List<MediaCover.MediaCover> Images { get; set; }
        public bool Monitored { get; set; }
    }

    public class SpeakarrBook
    {
        public string Title { get; set; }
        public string ForeignBookId { get; set; }
        public string Overview { get; set; }
        public List<MediaCover.MediaCover> Images { get; set; }
        public bool Monitored { get; set; }
        public SpeakarrAuthor Author { get; set; }
        public int AuthorId { get; set; }
        public List<SpeakarrEdition> Editions { get; set; }
    }

    public class SpeakarrProfile
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class SpeakarrTag
    {
        public string Label { get; set; }
        public int Id { get; set; }
    }

    public class SpeakarrRootFolder
    {
        public string Path { get; set; }
        public int Id { get; set; }
    }
}
