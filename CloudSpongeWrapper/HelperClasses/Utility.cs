using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using CloudSpongeWrapper.ExceptionHandler;
using Newtonsoft.Json;

namespace CloudSpongeWrapper.HelperClasses
{
    public static class Utility
    {
        public static T GetResponse<T>(this Uri uri, string domainKey, string domainPassword) where T: class 
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(uri.OriginalString);
            string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", domainKey, domainPassword)));

            request.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw ExceptionMapper.GetException(response.StatusCode);

                    using (var stream = new StreamReader(response.GetResponseStream()))
                    {
                        string fullResponse = stream.ReadToEnd();


                        if (CloudSponge.Format.Equals("json", StringComparison.OrdinalIgnoreCase))
                        {
                            return JsonConvert.DeserializeObject<T>(fullResponse);
                        }
                        else
                        {
                            return ParseXml<T>(fullResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ExceptionMapper.GetException(ex.Message);
            }
        }

        private static T ParseXml<T>(string response) where T : class
        {
            XDocument xDoc = XDocument.Parse(response,LoadOptions.PreserveWhitespace);

            Type objType = typeof(T);

            switch (objType.Name)
            {
                case "CSResponse":
                    CSResponse objResponse = (from e in xDoc.Elements("result")
                                              select new CSResponse()
                                              {
                                                  echo = e.Element("echo").Value,
                                                  import_id = Convert.ToInt32(e.Element("import-id").Value),
                                                  status = e.Element("status").Value,
                                                  url = e.Element("url").Value,
                                                  user_id = e.Element("user-id").Value
                                              }).FirstOrDefault();
                    return objResponse as T;
                case "CSImportResponse":
                    CSImportResponse objImportResponse = (from e in xDoc.Elements("result")
                                                          select new CSImportResponse()
                                              {
                                                  echo = e.Element("echo").Value,
                                                  import_id = Convert.ToInt32(e.Element("import-id").Value),
                                                  status = e.Element("status").Value,
                                                  user_id = e.Element("user-id").Value
                                              }).FirstOrDefault();
                    return objImportResponse as T;
                case "CSAppletResponse":
                    CSAppletResponse objAppletResponse = (from e in xDoc.Elements("result")
                                                          select new CSAppletResponse()
                                                          {
                                                              echo = e.Element("echo").Value,
                                                              import_id = Convert.ToInt32(e.Element("import-id").Value),
                                                              status = e.Element("status").Value,
                                                              user_id = e.Element("user-id").Value
                                                          }).FirstOrDefault();
                    return objAppletResponse as T;
                case "CSEventsResponse":
                    CSEventsResponse objEventResponse = (from e in xDoc.Elements("eventsResponse")
                                                         select new CSEventsResponse()
                                                         {
                                                             import_id = Convert.ToInt32(e.Element("import-id").Value),
                                                             user_id = e.Element("user-id").Value,
                                                             events = (from eventNode in e.Elements("events").Elements("event")
                                                                       select new CSEvent()
                                                                       {
                                                                           event_type = eventNode.Element("event-type").Value,
                                                                           status = eventNode.Element("status").Value,
                                                                           value = Convert.ToInt32(eventNode.Element("value").Value)

                                                                       }).ToList()
                                                         }).FirstOrDefault();
                    return objEventResponse as T;
                case "CSContactResponse":
                    CSContactResponse objContactResponse = (from e in xDoc.Elements("contactsResponse")
                                                            select new CSContactResponse()
                                                           {
                                                               import_id = Convert.ToInt32(e.Element("import-id").Value),
                                                               user_id = e.Element("user-id").Value,
                                                               echo = e.Element("echo").Value,
                                                               contacts_owner = (from contactOwner in e.Elements("contacts-owner")
                                                                                 select new ContactsOwner()
                                                                                 {
                                                                                     first_name = contactOwner.Element("first-name").Value,
                                                                                     last_name = contactOwner.Element("last-name").Value,
                                                                                     email = (from contactOwnerEmail in contactOwner.Element("email").Elements("email")
                                                                                              select new OwnerEmail()
                                                                                              {
                                                                                                  address = contactOwnerEmail.Element("address").Value
                                                                                              }).ToList()
                                                                                 }).FirstOrDefault(),
                                                               contacts = (from contactsNode in e.Element("contacts").Elements("contact")
                                                                           select new Contact()
                                                                           {
                                                                               first_name = contactsNode.Element("first-name").Value,
                                                                               last_name = contactsNode.Element("last-name").Value,
                                                                               email = (from contactEmail in contactsNode.Element("email").Elements("email")
                                                                                        select new Email()
                                                                                        {
                                                                                            address = contactEmail.Element("address").Value,
                                                                                            type = contactEmail.Element("type").Value
                                                                                        }).ToList(),
                                                                               addresses = (from contactEmail in contactsNode.Element("address").Elements("address")
                                                                                            select new Address()
                                                                                            {
                                                                                                street = contactEmail.Element("street").Value,
                                                                                                city = contactEmail.Element("city").Value,
                                                                                                country = contactEmail.Element("country").Value,
                                                                                                formatted = contactEmail.Element("formatted").Value,
                                                                                                postal_code = contactEmail.Element("postal-code").Value,
                                                                                                region = contactEmail.Element("region").Value

                                                                                            }).ToList(),
                                                                               phone = (from contactPhone in contactsNode.Element("phone").Elements("phone")
                                                                                        select new Phone()
                                                                                        {
                                                                                            number = contactPhone.Element("number").Value,
                                                                                            type = contactPhone.Element("type").Value
                                                                                        }).ToList(),
                                                                           }).ToList()
                                                           }).FirstOrDefault();
                    return objContactResponse as T;

            }

            return null;

        }

        public static string AppletTemlate(this Uri uri, int importId)
        {
            var template = new StringBuilder();

            template.Append("<!--[if !IE]> Firefox and others will use outer object -->");
            template.Append("<object classid=\"java:ContactsApplet\" type=\"application/x-java-applet\" archive=\"" + uri.OriginalString + "\" height=\"1\" width=\"1\">");

            template.Append("<!-- Konqueror browser needs the following param -->");
            template.Append("<param name=\"archive\" value=\"" + uri.OriginalString + "\" />");
            template.Append("<param name=\"cookieValue\" value=\"document.cookie\"/>");
            template.Append("<param name=\"importId\" value=\"" + importId + "\"/>");
            template.Append("<param name=\"MAYSCRIPT\" value=\"true\">");
            template.Append("<!--<![endif]-->");

            template.Append("<!-- MSIE (Microsoft Internet Explorer) will use inner object -->");
            template.Append("<object classid=\"clsid:8AD9C840-044E-11D1-B3E9-00805F499D93\" codebase=\"http://java.sun.com/update/1.6.0/jinstall-6u30-windows-i586.cab\" height=\"0\" width=\"0\" >");
            template.Append("<param name=\"code\" value=\"ContactsApplet\" />");
            template.Append("<param name=\"archive\" value=\"" + uri.OriginalString + "\" />");
            template.Append("<param name=\"cookieValue\" value=\"document.cookie\"/>");
            template.Append("<param name=\"importId\" value=\"" + importId + "\"/>");
            template.Append("<param name=\"MAYSCRIPT\" value=\"true\">");

            template.Append("<!-- Chrome falls through to this innermost applet -->");
            template.Append("<applet archive=\"" + uri.OriginalString + "\" code=\"ContactsApplet\" height=\"1\" width=\"1\" MAYSCRIPT>");
            template.Append("<param name=\"cookieValue\" value=\"document.cookie\" />");
            template.Append("<param name=\"importId\" value=\"" + importId + "\"/>");
            template.Append("<param name=\"MAYSCRIPT\" value=\"true\">");
            template.Append("<strong>This browser does not have a Java Plug-in.<br />");
            template.Append("<a href=\"http://java.sun.com/products/plugin/downloads/index.html\">");
            template.Append("Get the latest Java Plug-in here.");
            template.Append("</a></strong></applet></object>");
            template.Append("<!--[if !IE]> close outer object -->");
            template.Append("</object>");
            template.Append("<!--<![endif]-->");

            return template.ToString();
        }
    }
}
