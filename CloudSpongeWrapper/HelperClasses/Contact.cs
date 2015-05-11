using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudSpongeWrapper.HelperClasses
{
    public class Contact
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public List<Phone> phone { get; set; }
        public List<Email> email { get; set; }
        public List<Address> addresses { get; set; }
    }
}
