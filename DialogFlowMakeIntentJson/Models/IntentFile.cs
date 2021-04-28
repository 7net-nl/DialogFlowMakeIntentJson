using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DialogFlowMakeIntentJson.Models
{
    public class IntentFile
    {
        public string name { get; set; }
        public bool auto { get; set; }
        public List<IntentRespons> responses { get; set; }
        public List<IntentUserSays> userSays { get; set; }


    }
}
