using System.Collections.Generic;
using Speakarr.Http.REST;

namespace Speakarr.Api.V1.Localization
{
    public class LocalizationResource : RestResource
    {
        public Dictionary<string, string> Strings { get; set; }
    }

    public static class LocalizationResourceMapper
    {
        public static LocalizationResource ToResource(this Dictionary<string, string> localization)
        {
            if (localization == null)
            {
                return null;
            }

            return new LocalizationResource
            {
                Strings = localization,
            };
        }
    }
}
