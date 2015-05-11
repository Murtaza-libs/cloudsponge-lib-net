using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudSpongeWrapper.HelperClasses
{
    public class CSContactResponse
    {
        public object echo { get; set; }
        public string user_id { get; set; }
        public int import_id { get; set; }
        public ContactsOwner contacts_owner { get; set; }
        public List<Contact> contacts { get; set; }

        public static CSContactResponse ContactsResponse(int importId, string format, string echo = "")
        {
            string contactsRoot = string.Format("{0}contacts.{1}/", CloudSponge.Host, format);
            string contactsFormat = string.Format("{0}{{0}}?echo={{1}}", contactsRoot);

            string uri = string.Format(contactsFormat, importId, echo);

            return new Uri(uri).GetResponse<CSContactResponse>(CloudSponge.DomainKey, CloudSponge.DomainPassword);
        }
    }
}
