using System;
using System.Collections.Generic;
using NzbDrone.Core.Books;

namespace NzbDrone.Core.MetadataSource
{
    public interface IProvideAuthorInfo
    {
        Author GetAuthorInfo(string speakarrId, bool useCache = true);
        HashSet<string> GetChangedAuthors(DateTime startTime);
    }
}
