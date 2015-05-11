using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudSpongeWrapper.HelperClasses
{
    public class CSAppletResponse
    {
        public string status { get; set; }
        public string url { get; set; }
        public int import_id { get; set; }
        public string user_id { get; set; }
        public string echo { get; set; }

        public static CSResponse AppletResponse(ContactServiceApplet service, string format, string userId, string echo)
        {
            string desktopRoot = string.Format("{0}desktop_applet.{1}", CloudSponge.BeginImportRoot, format);
            string desktopFormat = string.Format("{0}?service={{0}}&user_id={{1}}&echo={{2}}", desktopRoot);

            string uri = string.Format(desktopFormat, service, userId, echo);

            return new Uri(uri).GetResponse<CSResponse>(CloudSponge.DomainKey, CloudSponge.DomainPassword);
        }
    }
}
