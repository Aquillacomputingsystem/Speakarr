using System.Collections.Generic;

namespace Speakarr.Http
{
    public class ApiInfoResource
    {
        public string Current { get; set; }
        public List<string> Deprecated { get; set; }
    }
}
