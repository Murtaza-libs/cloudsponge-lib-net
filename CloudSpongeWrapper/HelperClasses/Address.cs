using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudSpongeWrapper.HelperClasses
{
    public class Address
    {
        public string street { get; set; }
        public string city { get; set; }
        public string region { get; set; }
        public string country { get; set; }
        public string postal_code { get; set; }
        public string formatted { get; set; }
    }
}
