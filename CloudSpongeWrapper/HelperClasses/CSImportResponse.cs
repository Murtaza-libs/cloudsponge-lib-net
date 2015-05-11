using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudSpongeWrapper.HelperClasses
{
    public class CSImportResponse
    {
        public string status { get; set; }
        public int import_id { get; set; }
        public string user_id { get; set; }
        public string echo { get; set; }

        public static CSImportResponse ImportResponse(ContactServiceImport service, string format, string username, string password, string userId, string echo)
        {
            string importRoot = string.Format("{0}import.{1}", CloudSponge.BeginImportRoot, format);
            string importFormat = string.Format("{0}?service={{2}}&username={{0}}&password={{1}}&user_id={{3}}&echo={{4}}", importRoot);

            string uri = string.Format(importFormat, username, password, service, userId, echo);

            return new Uri(uri).GetResponse<CSImportResponse>(CloudSponge.DomainKey, CloudSponge.DomainPassword);
        }
    }
}
