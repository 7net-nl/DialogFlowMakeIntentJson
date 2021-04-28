using System.Collections.Generic;

namespace DialogFlowMakeIntentJson.Models
{
    public class IntentMessage
    {
        public string type { get; set; }
        public string lang { get; set; }
        public string condition { get; set; }
        public IList<string> speech { get; set; }
    }
}