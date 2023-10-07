using System;
using System.Collections.Generic;
using System.Net;
using FluentValidation.Results;
using Newtonsoft.Json;
using NLog;
using NzbDrone.Common.Extensions;
using NzbDrone.Common.Http;

namespace NzbDrone.Core.ImportLists.Speakarr
{
    public interface ISpeakarrV1Proxy
    {
        List<SpeakarrAuthor> GetAuthors(SpeakarrSettings settings);
        List<SpeakarrBook> GetBooks(SpeakarrSettings settings);
        List<SpeakarrProfile> GetProfiles(SpeakarrSettings settings);
        List<SpeakarrRootFolder> GetRootFolders(SpeakarrSettings settings);
        List<SpeakarrTag> GetTags(SpeakarrSettings settings);
        ValidationFailure Test(SpeakarrSettings settings);
    }

    public class SpeakarrV1Proxy : ISpeakarrV1Proxy
    {
        private readonly IHttpClient _httpClient;
        private readonly Logger _logger;

        public SpeakarrV1Proxy(IHttpClient httpClient, Logger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public List<SpeakarrAuthor> GetAuthors(SpeakarrSettings settings)
        {
            return Execute<SpeakarrAuthor>("/api/v1/author", settings);
        }

        public List<SpeakarrBook> GetBooks(SpeakarrSettings settings)
        {
            return Execute<SpeakarrBook>("/api/v1/book", settings);
        }

        public List<SpeakarrProfile> GetProfiles(SpeakarrSettings settings)
        {
            return Execute<SpeakarrProfile>("/api/v1/qualityprofile", settings);
        }

        public List<SpeakarrRootFolder> GetRootFolders(SpeakarrSettings settings)
        {
            return Execute<SpeakarrRootFolder>("api/v1/rootfolder", settings);
        }

        public List<SpeakarrTag> GetTags(SpeakarrSettings settings)
        {
            return Execute<SpeakarrTag>("/api/v1/tag", settings);
        }

        public ValidationFailure Test(SpeakarrSettings settings)
        {
            try
            {
                GetAuthors(settings);
            }
            catch (HttpException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.Error(ex, "API Key is invalid");
                    return new ValidationFailure("ApiKey", "API Key is invalid");
                }

                if (ex.Response.HasHttpRedirect)
                {
                    _logger.Error(ex, "Speakarr returned redirect and is invalid");
                    return new ValidationFailure("BaseUrl", "Speakarr URL is invalid, are you missing a URL base?");
                }

                _logger.Error(ex, "Unable to connect to import list.");
                return new ValidationFailure(string.Empty, $"Unable to connect to import list: {ex.Message}. Check the log surrounding this error for details.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unable to connect to import list.");
                return new ValidationFailure(string.Empty, $"Unable to connect to import list: {ex.Message}. Check the log surrounding this error for details.");
            }

            return null;
        }

        private List<TResource> Execute<TResource>(string resource, SpeakarrSettings settings)
        {
            if (settings.BaseUrl.IsNullOrWhiteSpace() || settings.ApiKey.IsNullOrWhiteSpace())
            {
                return new List<TResource>();
            }

            var baseUrl = settings.BaseUrl.TrimEnd('/');

            var request = new HttpRequestBuilder(baseUrl).Resource(resource)
                .Accept(HttpAccept.Json)
                .SetHeader("X-Api-Key", settings.ApiKey)
                .Build();

            var response = _httpClient.Get(request);

            if ((int)response.StatusCode >= 300)
            {
                throw new HttpException(response);
            }

            var results = JsonConvert.DeserializeObject<List<TResource>>(response.Content);

            return results;
        }
    }
}
