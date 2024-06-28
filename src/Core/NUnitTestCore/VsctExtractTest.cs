using System.Xml;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using RevEng.Core;

namespace UnitTests
{
    [TestFixture]
    public class VsctExtractTest
    {
        [Test]
        public void CanSplit()
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(System.IO.File.ReadAllText("C:\\Code\\Github\\EFCorePowerTools\\src\\GUI\\EFCorePowerTools\\EFCorePowerToolsPackage.en.vsct")); //myXmlString is the xml file in string //copying xml to string: string myXmlString = xmldoc.OuterXml.ToString();

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/VisualStudio/2005-10-18/CommandTable");
            XmlNodeList xnList = xml.SelectNodes("ns:CommandTable/ns:Commands/ns:Buttons/ns:Button", nsmgr);
            foreach (XmlNode xn in xnList)
            {
                XmlNode text = xn.SelectSingleNode("ns:Strings/ns:ButtonText", nsmgr);
                if (text != null)
                {
                    var id = xn.Attributes["id"].InnerText;
                    var textValue = text.InnerText;
                }
            }
        }
    }
}
