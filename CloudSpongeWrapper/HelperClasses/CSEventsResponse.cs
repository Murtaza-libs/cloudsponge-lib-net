using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudSpongeWrapper.HelperClasses
{
    public enum EventType
    {
        INITIALIZING,
        GATHERING,
        COMPLETE
    }

    public enum EventStatus
    {
        INPROGRESS,
        COMPLETED,
        ERROR
    }

    public class CSEventsResponse
    {
        public List<CSEvent> events { get; set; }
        public int import_id { get; set; }
        public object user_id { get; set; }
        public int id { get; set; }

        private static readonly Dictionary<int, string> ErrorList = new Dictionary<int, string>()
        {
            {1,"Could not authenticate the domain key/password."},
            {2,"Invalid parameters supplied to begin_import."},
            {256,"Unexpected error occurred during webmail import."},
            {257,"Webmail import failed."},
            {258,"Timeout occurred during webmail import."},
            {259,"Username and password are required."},
            {260,"Service is required."},
            {261,"Unrecognized service selected."},
            {262,"The same import failed to authenticate recently."},
            {263,"Username and password do not match."},
            {264,"The address book is temporarily unavailable, please try again later."},
            {265,"This account has been canceled."},
            {266,"The account has been blocked. Reset the password to reenable it."},
            {267,"Terms of Service have changed for your account. Sign in to your account to enable it."},
            {512,"Unknown error occurred during a user consent import."},
            {513,"User consent import failed because the domain is not permitted to use the service."},
            {514,"User consent import failed because the user did not provide consent to access their contacts."},
            {516,"Consent was not granted within the allotted time."},
            {517,"The user abandoned the import before consent was granted."},
            {518,"Unable to communicate successfully with the address book provider."},
            {528,"Unable to retrieve contacts. New Yahoo! users must wait 14 days to use this feature."},
            {768,"Unexpected error occurred during applet import."},
            {769,"Applet failed to import because user did not trust the applet."},
            {770,"Applet failed to import because it could not find an appropriate address book to import."},
            {771,"Applet failed to submit contacts to CloudSponge."},
            {772,"The Desktop Applet was not trusted within the allotted time."},
            {773,"The user abandoned the import before the Desktop Applet was trusted."},
            {774,"You must allow access to Microsoft Outlook to import your contacts."},
            {775,"The import was denied access to Contacts by the OS."},
            {1025,"A file was uploaded that is not of type text/csv."},
            {1026,"A CSV file was uploaded but could not be parsed."}
        };

        public bool IsCompleted
        {
            get
            {
                return (from objEvent in events
                        where (objEvent.event_type == EventType.COMPLETE.ToString()) &&
                              (objEvent.status == EventStatus.COMPLETED.ToString())
                        select objEvent).Any();
            }
        }

        public bool IsError
        {
            get
            {
                return (from objEvent in events
                        where (objEvent.status == EventStatus.ERROR.ToString())
                        select objEvent).Any();
            }
        }

        public string GetErrorDescription()
        {
            int errorCode = (from e in events
                             where (e.status == EventStatus.ERROR.ToString())
                             select e.value).FirstOrDefault();

            return (from e in ErrorList
                    where e.Key == errorCode
                    select e.Value).FirstOrDefault();
        }

        public CSEventsResponse Events(int importId, string format, string echo = "")
        {
            //UNIX Time Stamp
            double unixTimestamp = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string eventsRoot = string.Format("{0}events.{1}/", CloudSponge.Host, format);
            string eventsFormat = string.Format("{0}{{0}}?echo={{1}}&unix_second_resolution_timestramp={{2}}", eventsRoot);

            string uri = string.Format(eventsFormat, importId, echo, unixTimestamp);
            return new Uri(uri).GetResponse<CSEventsResponse>(CloudSponge.DomainKey, CloudSponge.DomainPassword);
        }
    }
}
