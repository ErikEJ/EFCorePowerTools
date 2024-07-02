using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using System.IO;
using System.Linq;

namespace UnitTests
{
    [TestFixture]
    public class VsctExtractTest
    {
        [Test, Ignore("Run locally only")]
        public void CanExtract()
        {
            var buttonTexts = new List<Buttontexts>();
            var files = Directory.EnumerateFiles("C:\\Code\\Github\\EFCorePowerTools\\src\\GUI\\EFCorePowerTools", "EFCorePowerToolsPackage.*.vsct");

            foreach (var file in files)
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(File.ReadAllText(file));

                var language = Path.GetFileNameWithoutExtension(file).Replace("EFCorePowerToolsPackage.", "").Replace(".vsct", "");

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
                nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/VisualStudio/2005-10-18/CommandTable");
                XmlNodeList xnList = xml.SelectNodes("ns:CommandTable/ns:Commands/ns:Buttons/ns:Button", nsmgr);
                foreach (XmlNode xn in xnList)
                {
                    XmlNode text = xn.SelectSingleNode("ns:Strings/ns:ButtonText", nsmgr);
                    if (text != null)
                    {
                        buttonTexts.Add(new Buttontexts
                        {
                            Id = xn.Attributes["id"].InnerText,
                            Text = text.InnerText,
                            Locale = language,
                        });
                    }
                }
            }

            var content = buttonTexts.Select(b => b.ToString()).OrderBy(x => x).ToList();
            File.WriteAllLines("C:\\Code\\Github\\EFCorePowerTools\\src\\GUI\\EFCorePowerTools\\VsctExtractByLocale.csv", content);
        }

        private class Buttontexts
        {
            public string Id { get; set; }
            public string Text { get; set; }
            public string Locale { get; set; }

            public override string ToString()
            {
                return $"{Locale},{Id},{Text}";
            }
        }
    }
}
