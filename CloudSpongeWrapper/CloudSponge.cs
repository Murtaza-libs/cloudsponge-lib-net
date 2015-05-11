using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudSpongeWrapper.HelperClasses;

namespace CloudSpongeWrapper
{
    #region " ENUMS "

    public enum ResponseFormat
    {
        XML,
        JSON
    }

    public enum ContactServiceConsent
    {
        FACEBOOK,       //Facebook.com
        LINKEDIN,       //LinkedIn.com
        GMAIL,          //Gmail.com
        YAHOO,          //Yahoo.com
        WINDOWSLIVE,    //Outlook.com
        AOL,            //aol.com
        MAIL_RU,        //mail.ru
        OUTLOOK,        // CSV Import
        ADDRESSBOOK     // VCard Import
    }

    public enum ContactServiceImport
    {
        PLAXO,
        UOL,
        BOL,
        TERRA,
        ICLOUD,
        REDIFF,
        GMX,
        MAIL126,
        MAIL163,
        MAIL_YEAH_NET,
        SAPO
    }

    public enum ContactServiceApplet
    {
        OUTLOOK,
        ADDRESSBOOK
    }

    #endregion

    public class CloudSponge
    {
        #region "Properties"

        public const string Host = "https://api.cloudsponge.com/";

        public static readonly string BeginImportRoot = string.Format("{0}begin_import/", Host);

        public static string DomainKey { get; private set; }
        public static string DomainPassword { get; private set; }

        public string UserId { get; private set; }
        public string Echo { get; private set; }

        public static string Format { get; private set; }

        #endregion

        #region "Constructors"

        public CloudSponge()
        {

        }

        public CloudSponge(string domainKey, string domainPassword, ResponseFormat format = ResponseFormat.JSON)
        {
            DomainKey = domainKey;
            DomainPassword = domainPassword;
            Format = format.ToString().ToLower();
        }

        #endregion

        #region "Function"


        public CSResponse GetConsent(ContactServiceConsent service, string userId = "", string echo = "")
        {
            UserId = userId;
            Echo = echo;

            return CSResponse.ConsentResponse(service, Format, userId, echo);
        }

        public CSImportResponse GetImport(ContactServiceImport service, string username, string password, string userId = "", string echo = "")
        {
            UserId = userId;
            Echo = echo;

            return CSImportResponse.ImportResponse(service, Format, username, password, userId, echo);
        }

        public string GetDesktopApplet(ContactServiceApplet service, string userId = "", string echo = "")
        {
            UserId = userId;
            Echo = echo;

            var response = CSAppletResponse.AppletResponse(service, Format, userId, echo);

            return new Uri(response.url).AppletTemlate(response.import_id);
        }

        public CSContactResponse GetContacts(int importId, string echo = "")
        {
            Echo = echo;

            return CSContactResponse.ContactsResponse(importId, Format);
        }

        public CSEventsResponse GetEvents(int importId, string echo = "")
        {
            Echo = echo;

            return new CSEventsResponse().Events(importId, Format, echo);
        }

        #endregion
    }
}
