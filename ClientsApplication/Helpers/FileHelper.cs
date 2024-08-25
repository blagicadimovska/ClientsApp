using ClientsApplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace ClientsApplication.Helpers
{
    public static class FileHelper
    {
        public static List<Client> ParseXml(Stream xmlStream)
        {
            var clients = new List<Client>();

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlStream);

            foreach (XmlNode clientNode in xmlDoc.SelectNodes("//Client"))
            {
                var client = new Client
                {
                    ClientID = int.Parse(clientNode.Attributes["ID"].Value),
                    Name = clientNode.SelectSingleNode("Name").InnerText,
                    BirthDate = DateTime.Parse(clientNode.SelectSingleNode("BirthDate").InnerText),
                    Addresses = new List<Address>()
                };

                foreach (XmlNode addressNode in clientNode.SelectNodes("Addresses/Address"))
                {
                    var address = new Address
                    {
                        Type = int.Parse(addressNode.Attributes["Type"].Value),
                        ClientAddress = addressNode.InnerText
                    };

                    client.Addresses.Add(address);
                }

                clients.Add(client);
            }

            return clients;
        }
    }
}
