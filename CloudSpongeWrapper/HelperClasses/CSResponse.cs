using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CloudSpongeWrapper.HelperClasses
{
    public class CSResponse
    {
        public string status { get; set; }
        public string url { get; set; }
        public int import_id { get; set; }
        public string user_id { get; set; }
        public string echo { get; set; }

        public static CSResponse ConsentResponse(ContactServiceConsent service, string format, string userId, string echo)
        {
            string consentRoot = string.Format("{0}user_consent.{1}", CloudSponge.BeginImportRoot, format);
            string consentFormat = string.Format("{0}?service={{0}}&user_id={{1}}&echo={{2}}", consentRoot);

            string uri = string.Format(consentFormat, service, userId, echo);

            return new Uri(uri).GetResponse<CSResponse>(CloudSponge.DomainKey, CloudSponge.DomainPassword);
        }

        
    }
}
