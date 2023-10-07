using System;

namespace NzbDrone.Common.Exceptions
{
    public class SpeakarrStartupException : NzbDroneException
    {
        public SpeakarrStartupException(string message, params object[] args)
            : base("Speakarr failed to start: " + string.Format(message, args))
        {
        }

        public SpeakarrStartupException(string message)
            : base("Speakarr failed to start: " + message)
        {
        }

        public SpeakarrStartupException()
            : base("Speakarr failed to start")
        {
        }

        public SpeakarrStartupException(Exception innerException, string message, params object[] args)
            : base("Speakarr failed to start: " + string.Format(message, args), innerException)
        {
        }

        public SpeakarrStartupException(Exception innerException, string message)
            : base("Speakarr failed to start: " + message, innerException)
        {
        }

        public SpeakarrStartupException(Exception innerException)
            : base("Speakarr failed to start: " + innerException.Message)
        {
        }
    }
}
