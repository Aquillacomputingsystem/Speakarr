using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NzbDrone.Core.Languages;
using Speakarr.Http;
using Speakarr.Http.REST;

namespace Speakarr.Api.V1.Languages
{
    [V1ApiController]
    public class LanguageController : RestController<LanguageResource>
    {
        protected override LanguageResource GetResourceById(int id)
        {
            var language = (Language)id;

            return new LanguageResource
            {
                Id = (int)language,
                Name = language.ToString()
            };
        }

        [HttpGet]
        public List<LanguageResource> GetAll()
        {
            return Language.All.Select(l => new LanguageResource
            {
                Id = (int)l,
                Name = l.ToString()
            })
                              .OrderBy(l => l.Name)
                              .ToList();
        }
    }
}
