# cloudsponge

Simple .Net wrapper for www.cloudsponge.com's REST API.

# Usage Example

In case of XML Format
        
        CloudSponge _clousponge  = new CloudSponge("DomainKey","DomainPassword", ResponseFormat.XML);

In case of JSON Format
        
        CloudSponge _clousponge  = new CloudSponge("DomainKey","DomainPassword");

Although it does not matter, as I map both response to objects for easy usage and intellicense. 

You can use these functions:

It will get the redirect URL in case USER CONSENT e.g Gmail, Facebook etc

        public List<string> GetRedirectUrl(ContactServiceConsent source, CloudSponge cloudsponge)
        {
            var data = new List<string>();

            CSResponse csResponse = cloudsponge.GetConsent(source);

            if (csResponse.status.Equals("success", StringComparison.OrdinalIgnoreCase))
            {
                data.Add(csResponse.import_id.ToString());
                data.Add(csResponse.url);
            }

            return data;
        }

This function will get you Contacts for User source, you can call this function after above function.
Once you have the import-id from above function response
        
        public List<Contacts> GetContacsForConsent(CloudSponge cloudsponge, int importId)
        {
            var objItems = new List<Contacts>();

            var csEventsResponse = new CSEventsResponse();

            bool isSuccess = false;

            while (!isSuccess)
            {
                csEventsResponse = cloudsponge.GetEvents(importId);

                isSuccess = csEventsResponse.IsError ? csEventsResponse.IsError : csEventsResponse.IsCompleted;
            }

            if (csEventsResponse.IsError)
            {
                string errorDescription = csEventsResponse.GetErrorDescription();

                var oContact = new Contacts() { status = "fail", errorDescription = errorDescription };

                objItems.Add(oContact);
            }
            else
            {
                CSContactResponse csContacts = cloudsponge.GetContacts(importId);

                csContacts.contacts.ForEach(x =>
                {

                    var objContact = new Contacts();

                    var objEmails = new List<Email>();
                    var objPhone = new List<Phone>();

                    objContact.status = "success";
                    objContact.FirstName = x.first_name;
                    objContact.LastName = x.last_name;

                    if (x.email != null)
                    {
                        if (x.email.Count > 0)
                        {
                            x.email.ForEach(objEmails.Add);
                        }
                    }

                    if (x.phone != null)
                    {
                        if (x.phone.Count > 0)
                        {
                            x.phone.ForEach(objPhone.Add);
                        }
                    }

                    objContact.EmailList = objEmails;
                    objContact.PhoneList = objPhone;

                    objItems.Add(objContact);


                });
            }


            return objItems;
        }

This function will get you Contacts for User source, which does not require user consent like Plaxo, GMX etc
        
        public List<Contacts> GetContacsForImport(ContactServiceImport source, string userName, string password, CloudSponge cloudsponge)
        {
            var objItems = new List<Contacts>();

            CSImportResponse csResponse = cloudsponge.GetImport(source, userName, password);

            var csEventsResponse = new CSEventsResponse();

            if (csResponse.status.Equals("success", StringComparison.OrdinalIgnoreCase))
            {
                int importId = csResponse.import_id;

                bool isSuccess = false;

                while (!isSuccess)
                {
                    csEventsResponse = cloudsponge.GetEvents(importId);

                    isSuccess = csEventsResponse.IsError ? csEventsResponse.IsError : csEventsResponse.IsCompleted;
                }

                if (csEventsResponse.IsError)
                {
                    string errorDescription = csEventsResponse.GetErrorDescription();

                    var oContact = new Contacts() { status = "fail", errorDescription = errorDescription };

                    objItems.Add(oContact);
                }
                else
                {
                    CSContactResponse csContacts = cloudsponge.GetContacts(importId);

                    csContacts.contacts.ForEach(x =>
                    {

                        var objContact = new Contacts();

                        var objEmails = new List<Email>();
                        var objPhone = new List<Phone>();

                        objContact.status = "success";
                        objContact.FirstName = x.first_name;
                        objContact.LastName = x.last_name;

                        if (x.email.Count > 0)
                        {
                            x.email.ForEach(objEmails.Add);
                        }

                        if (x.phone.Count > 0)
                        {
                            x.phone.ForEach(objPhone.Add);
                        }

                        objContact.EmailList = objEmails;
                        objContact.PhoneList = objPhone;

                        objItems.Add(objContact);


                    });
                }
            }

            return objItems;
        }

This is the CONTACT class I have used in above function

    public class Contacts
    {
        public List<Email> EmailList { get; set; }
        public List<Phone> PhoneList { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string status { get; set; }
        public string errorDescription { get; set; }
    }