using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudSpongeWrapper.HelperClasses
{
    public class ContactsOwner
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public List<OwnerEmail> email { get; set; }
    }
}
