using NzbDrone.Common.Http;

namespace NzbDrone.Common.Cloud
{
    public interface ISpeakarrCloudRequestBuilder
    {
        IHttpRequestBuilderFactory Services { get; }
        IHttpRequestBuilderFactory Metadata { get; }
    }

    public class SpeakarrCloudRequestBuilder : ISpeakarrCloudRequestBuilder
    {
        public SpeakarrCloudRequestBuilder()
        {
            //TODO: Create Update Endpoint
            Services = new HttpRequestBuilder("https://speakarr.servarr.com/v1/")
                .CreateFactory();

            Metadata = new HttpRequestBuilder("https://api.bookinfo.club/v1/{route}")
                .CreateFactory();
        }

        public IHttpRequestBuilderFactory Services { get; }

        public IHttpRequestBuilderFactory Metadata { get; }
    }
}
