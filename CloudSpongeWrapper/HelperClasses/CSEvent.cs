using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudSpongeWrapper.HelperClasses
{
    public class CSEvent
    {
        public int import_action_id { get; set; }
        public int import_id { get; set; }
        public string event_type { get; set; }
        public int value { get; set; }
        public object description { get; set; }
        public string status { get; set; }
        public string created_at { get; set; }
    }
}
